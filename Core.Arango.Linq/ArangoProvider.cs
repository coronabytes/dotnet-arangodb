using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using Core.Arango.Linq.Internal;

namespace Core.Arango.Linq
{
    public class ArangoProvider : IQueryProvider
    {
        private readonly ArangoContext _arango;
        private readonly string _collection;
        private readonly ArangoHandle _handle;

        public ArangoProvider(ArangoContext arango, ArangoHandle handle, string collection)
        {
            _arango = arango;
            _handle = handle;
            _collection = collection;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);
            try
            {
                return
                    (IQueryable) Activator.CreateInstance(typeof(ArangoQueryableContext<>).MakeGenericType(elementType),
                        this, expression);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new ArangoQueryableContext<TElement>(this, expression);
        }

        public object Execute(Expression expression)
        {
            return default;
        }

        /// <summary>
        /// Wird beim Ausführen der Query (z.B. durch .ToList()) aufgerufen. Wandelt den ExpressionTree in ein Arango-Query um, bindet Variablen und feuert es dann gegen die Datenbank ab.
        /// </summary>
        /// <typeparam name="TResult">Typ des angefragten Objekts</typeparam>
        /// <param name="expression">Expression-Tree, der sich aus dem LINQ-Query ergibt</param>
        /// <returns>Ergebnis der DB-Abfrage</returns>
        public TResult Execute<TResult>(Expression expression)
        {
            var type = typeof(TResult);
            var isEnumerable = typeof(TResult).Name == "IEnumerable`1";

            var elementType = TypeSystem.GetElementType(expression.Type);

            var writer = new AqlCodeWriter(expression)
            {
                Collection = _collection
            };

            var query = writer.ToString();
            var bindVars = writer.BindVars;

            var res = _arango.QueryAsync(elementType, isEnumerable, _handle, query, bindVars).Result;
            var resType = res.GetType();

            return (TResult) res;
        }

        public async IAsyncEnumerator<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancel)
        {
            var elementType = TypeSystem.GetElementType(expression.Type);

            var writer = new AqlCodeWriter(expression);

            var query = writer.ToString();
            var bindVars = writer.BindVars;

            yield return (TResult) await _arango.QueryAsync(elementType, true, _handle, query, bindVars,
                cancellationToken: cancel);
        }
    }
}