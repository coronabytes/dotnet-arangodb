using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Linq.Interface;
using Core.Arango.Linq.Query;

namespace Core.Arango.Linq
{
    public static class ArangoQueryableExtensions
    {
        /*Relinq Extentions*/

        private static readonly ConcurrentDictionary<string, MethodInfo> cachedExtentions = new();

        static ArangoQueryableExtensions()
        {
            var extention = typeof(ArangoQueryableExtensions).GetRuntimeMethods()
                .Union(typeof(ArangoTraversalExtensions).GetRuntimeMethods())
                .Union(typeof(ArangoShortestPathExtensions).GetRuntimeMethods())
                .Where(x => x.GetCustomAttribute<ExtentionIdentifierAttribute>() != null)
                .GroupBy(x => x.GetCustomAttribute<ExtentionIdentifierAttribute>().Identifier)
                .Select(g => new {g.Key, Count = g.Count()})
                .Where(x => x.Count > 1)
                .FirstOrDefault();

            if (extention != null)
                throw new InvalidOperationException($"Multiple extention identifier {extention.Key} found");
        }

        internal static ArangoQueryable<T> AsArangoQueryable<T>(this IQueryable<T> source)
        {
            if (source == null)
                throw new InvalidCastException("Queryable source is null");

            var queryable = source as ArangoQueryable<T>;

            if (queryable == null)
                throw new InvalidCastException("Queryable source is not type of ArangoQueryable");

            return queryable;
        }

        public static (string aql, IDictionary<string, object> bindVars) ToAql<T>(this IQueryable<T> source)
        {
            var data = source.AsArangoQueryable().GetQueryData();
            return (data.Query.Trim(), data.BindVars);
        }

        public static Task<TSource> FirstAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return FirstOrDefaultAsync(source, false, null, cancellationToken);
        }

        public static Task<TSource> FirstAsync<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return FirstOrDefaultAsync(source, false, predicate, cancellationToken);
        }

        public static Task<TSource> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return FirstOrDefaultAsync(source, true, null, cancellationToken);
        }

        public static Task<TSource> FirstOrDefaultAsync<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return FirstOrDefaultAsync(source, true, predicate, cancellationToken);
        }

        private static Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> source, bool returnDefaultWhenEmpty,
            Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (predicate != null)
                source = source.Where(predicate);

            source = source.Take(1);

            return returnDefaultWhenEmpty ? source.AsArangoQueryable().FirstOrDefaultAsync(cancellationToken)
                : source.AsArangoQueryable().FirstAsync(cancellationToken);
        }

        public static Task<TSource> SingleAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return SingleOrDefaultAsync(source, false, null, cancellationToken);
        }

        public static Task<TSource> SingleAsync<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return SingleOrDefaultAsync(source, false, predicate, cancellationToken);
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            return SingleOrDefaultAsync(source, true, null, cancellationToken);
        }

        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return SingleOrDefaultAsync(source, true, predicate, cancellationToken);
        }

        private static Task<T> SingleOrDefaultAsync<T>(this IQueryable<T> source, bool returnDefaultWhenEmpty,
            Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            if (predicate != null)
                source = source.Where(predicate);

            source = source.Take(2);

            return returnDefaultWhenEmpty ? source.AsArangoQueryable().SingleOrDefaultAsync(cancellationToken) 
                : source.AsArangoQueryable().SingleAsync(cancellationToken);
        }

        public static Task<List<T>> ToListAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            return source.AsArangoQueryable().ToListAsync(cancellationToken);
        }

        internal static MethodInfo FindExtention(string identifier, params Type[] arguments)
        {
            var key = $"{identifier}_{string.Join("_", arguments.Select(x => x.FullName))}";

            return cachedExtentions
                .GetOrAdd(key,
                    typeof(ArangoQueryableExtensions).GetRuntimeMethods()
                        .Union(typeof(ArangoTraversalExtensions).GetRuntimeMethods())
                        .Union(typeof(ArangoShortestPathExtensions).GetRuntimeMethods())
                        .ToList()
                        .First(x => x.GetCustomAttribute<ExtentionIdentifierAttribute>()?.Identifier == identifier)
                        .MakeGenericMethod(arguments));
        }

        [ExtentionIdentifier("For")]
        public static IQueryable<TResult> For<TSource, TResult>(this IEnumerable<TSource> source,
            Expression<Func<TSource, IEnumerable<TResult>>> selector)
        {
            var queryableSource = source.AsQueryable();

            return queryableSource.Provider.CreateQuery<TResult>(
                Expression.Call(
                    FindExtention("For", typeof(TSource), typeof(TResult)),
                    queryableSource.Expression,
                    Expression.Quote(selector)
                ));
        }

        /*[ExtentionIdentifier("Collect")]
        public static IQueryable<IGrouping<TKey, TSource>> Collect<TSource, TKey>(this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector)
        {
            return source.Provider.CreateQuery<IGrouping<TKey, TSource>>(
                Expression.Call(
                    FindExtention("Collect", typeof(TSource), typeof(TKey)),
                    source.Expression,
                    Expression.Quote(keySelector)));
        }*/

        // TODO: Limit Broken?

        /*[ExtentionIdentifier("Limit")]
        public static IQueryable<TSource> Limit<TSource>(this IQueryable<TSource> source, int offset, int count)
        {
            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    FindExtention("Limit", typeof(TSource)),
                    source.Expression,
                    Expression.Constant(count),
                    Expression.Constant(offset)));
        }

        public static IQueryable<TSource> Limit<TSource>(this IQueryable<TSource> source, int count)
        {
            return Limit(source, 0, count);
        }*/

        public static IAqlModifiable<TSource> Update<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, object>> withSelector)
        {
            return Update(source, withSelector, null);
        }

        public static IAqlModifiable<TSource> Update<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, object>> withSelector
            , Expression<Func<TSource, object>> keySelector)
        {
            return UpdateReplace(source, withSelector, keySelector, "update");
        }

        public static IAqlModifiable<TSource> Replace<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, object>> withSelector)
        {
            return Replace(source, withSelector, null);
        }

        public static IAqlModifiable<TSource> Replace<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, object>> withSelector
            , Expression<Func<TSource, object>> keySelector)
        {
            return UpdateReplace(source, withSelector, keySelector, "replace");
        }

        [ExtentionIdentifier("UpdateReplace")]
        internal static IAqlModifiable<TSource> UpdateReplace<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, object>> withSelector
            , Expression<Func<TSource, object>> keySelector, string command)
        {
            if (keySelector == null)
                keySelector = x => null;

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    FindExtention("UpdateReplace", typeof(TSource)),
                    source.Expression,
                    Expression.Quote(withSelector),
                    Expression.Quote(keySelector),
                    Expression.Constant(command)
                )) as IAqlModifiable<TSource>;
        }

        public static IAqlModifiable<Aql> Upsert<TSource>(this IQueryable source,
            Expression<Func<Aql, object>> searchExpression,
            Expression<Func<Aql, object>> insertExpression, Expression<Func<Aql, TSource, object>> updateExpression)
        {
            return InternalUpsert(source.OfType<Aql>(), searchExpression, insertExpression, updateExpression,
                typeof(TSource));
        }

        public static IAqlModifiable<TSource> Upsert<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, object>> searchExpression,
            Expression<Func<TSource, object>> insertExpression,
            Expression<Func<TSource, TSource, object>> updateExpression)
        {
            return InternalUpsert(source, searchExpression, insertExpression, updateExpression, typeof(TSource));
        }

        [ExtentionIdentifier("InternalUpsert")]
        internal static IAqlModifiable<TSource> InternalUpsert<TSource, TOld>(this IQueryable<TSource> source,
            Expression<Func<TSource, object>> searchExpression,
            Expression<Func<TSource, object>> insertExpression,
            Expression<Func<TSource, TOld, object>> updateExpression, Type updateType)
        {
            var newUpdateExp = ExpressionParameterRewriter.RewriteParameterAt(updateExpression, 1, "OLD");

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    FindExtention("InternalUpsert", typeof(TSource), typeof(TOld)),
                    source.Expression,
                    Expression.Quote(searchExpression),
                    Expression.Quote(insertExpression),
                    Expression.Quote(newUpdateExp),
                    Expression.Constant(updateType)
                )) as IAqlModifiable<TSource>;
        }

        public static IAqlModifiable<TSource> Insert<TSource>(this IQueryable<TSource> source)
        {
            return Insert(source, null);
        }

        public static IAqlModifiable<TSource> Insert<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, object>> selector)
        {
            return Insert(source, selector, typeof(TSource));
        }

        [ExtentionIdentifier("Insert")]
        internal static IAqlModifiable<TSource> Insert<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, object>> selector, Type type)
        {
            if (selector == null)
                selector = x => null;

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    FindExtention("Insert", typeof(TSource)),
                    source.Expression,
                    Expression.Quote(selector),
                    Expression.Constant(type)
                )) as IAqlModifiable<TSource>;
        }

        public static IAqlModifiable<TSource> Remove<TSource>(this IQueryable<TSource> source)
        {
            return Remove(source, null);
        }

        public static IAqlModifiable<TSource> Remove<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, object>> keySelector)
        {
            return Remove(source, keySelector, typeof(TSource));
        }

        [ExtentionIdentifier("Remove")]
        internal static IAqlModifiable<TSource> Remove<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, object>> keySelector, Type type)
        {
            if (keySelector == null)
                keySelector = x => null;

            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    FindExtention("Remove", typeof(TSource)),
                    source.Expression,
                    Expression.Quote(keySelector),
                    Expression.Constant(type)
                )) as IAqlModifiable<TSource>;
        }

        [ExtentionIdentifier("In")]
        public static IAqlModifiable<TResult> In<TResult>(this IAqlModifiable source)
        {
            return source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    FindExtention("In", typeof(TResult)),
                    source.Expression
                )) as IAqlModifiable<TResult>;
        }

        [ExtentionIdentifier("SelectModification")]
        public static IQueryable<TResult> Select<TSource, TResult>(this IAqlModifiable<TSource> source,
            Expression<Func<TSource, TSource, TResult>> selector)
        {
            var newSelector = ExpressionParameterRewriter.RewriteParameters(selector, "NEW", "OLD");

            return source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    FindExtention("SelectModification", typeof(TSource), typeof(TResult)),
                    source.Expression,
                    Expression.Quote(newSelector)));
        }

        [ExtentionIdentifier("IgnoreModificationSelect")]
        internal static IQueryable<TResult> IgnoreModificationSelect<TResult>(this IAqlModifiable<TResult> source)
        {
            return source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    FindExtention("IgnoreModificationSelect", typeof(TResult)),
                    source.Expression));
        }

        [ExtentionIdentifier("Return")]
        public static IQueryable<TResult> Return<TSource, TResult>(this IQueryable<TSource> source,
            Expression<Func<TSource, TResult>> selector)
        {
            return source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    FindExtention("Return", typeof(TSource), typeof(TResult)),
                    source.Expression,
                    Expression.Quote(selector)));
        }

        [ExtentionIdentifier("Sort")]
        public static IOrderedQueryable<TSource> Sort<TSource, TKey>(this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector)
        {
            return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    FindExtention("Sort", typeof(TSource), typeof(TKey)),
                    source.Expression,
                    Expression.Quote(keySelector)));
        }

        public static IOrderedQueryable<TSource> SortDescending<TSource, TKey>(this IEnumerable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector)
        {
            return source.AsQueryable().SortDescending(keySelector);
        }

        [ExtentionIdentifier("SortDescending")]
        public static IOrderedQueryable<TSource> SortDescending<TSource, TKey>(this IQueryable<TSource> source,
            Expression<Func<TSource, TKey>> keySelector)
        {
            return (IOrderedQueryable<TSource>) source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    FindExtention("SortDescending", typeof(TSource), typeof(TKey)),
                    source.Expression,
                    Expression.Quote(keySelector)));
        }

        [ExtentionIdentifier("Let")]
        public static IQueryable<TResult> Let<TSource, TLet, TResult>(this IQueryable<TSource> source,
            Expression<Func<TSource, TLet>> letSelector,
            Expression<Func<IQueryable<TSource>, TLet, IQueryable<TResult>>> querySelector)
        {
            return source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    FindExtention("Let", typeof(TSource), typeof(TLet), typeof(TResult)),
                    source.Expression,
                    Expression.Quote(letSelector),
                    Expression.Quote(querySelector)));
        }

        [ExtentionIdentifier("Filter")]
        public static IQueryable<TSource> Filter<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate)
        {
            return source.Provider.CreateQuery<TSource>(
                Expression.Call(
                    FindExtention("Filter", typeof(TSource)),
                    source.Expression,
                    Expression.Quote(predicate)));
        }
    }
}