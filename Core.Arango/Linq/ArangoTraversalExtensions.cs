using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Arango.Linq.Collection;
using Core.Arango.Linq.Data;
using Core.Arango.Linq.Interface;
using Core.Arango.Linq.Query;

namespace Core.Arango.Linq
{
    public static class ArangoTraversalExtensions
    {
        [ExtentionIdentifier("Traversal_Selector")]
        public static ITraversalQueryable<TraversalData<TVertex, TEdge>> Traversal<TVertex, TEdge>(
            this IQueryable source, Expression<Func<string>> startVertex)
        {
            return source.Provider.CreateQuery<TraversalData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("Traversal_Selector", typeof(TVertex), typeof(TEdge)),
                    source.Expression,
                    Expression.Quote(startVertex))) as ITraversalQueryable<TraversalData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("Traversal_Constant")]
        public static ITraversalQueryable<TraversalData<TVertex, TEdge>> Traversal<TVertex, TEdge>(
            this IQueryable source, string startVertex)
        {
            return source.Provider.CreateQuery<TraversalData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("Traversal_Constant", typeof(TVertex), typeof(TEdge)),
                    source.Expression,
                    Expression.Constant(startVertex))) as ITraversalQueryable<TraversalData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("Traversal_Graph")]
        public static ITraversalQueryable<TraversalData<TVertex, TEdge>> Graph<TVertex, TEdge>(
            this ITraversalQueryable<TraversalData<TVertex, TEdge>> source, string graphName)
        {
            return source.Provider.CreateQuery<TraversalData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("Traversal_Graph", typeof(TVertex), typeof(TEdge)),
                    source.Expression,
                    Expression.Constant(graphName)
                )) as ITraversalQueryable<TraversalData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("Traversal_Edge_Direction")]
        public static ITraversalQueryable<TraversalData<TVertex, TEdge>> Edge<TVertex, TEdge>(
            this ITraversalQueryable<TraversalData<TVertex, TEdge>> source, string collectionName,
            EdgeDirection direction)
        {
            return source.Provider.CreateQuery<TraversalData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("Traversal_Edge_Direction", typeof(TVertex), typeof(TEdge)),
                    source.Expression,
                    Expression.Constant(collectionName),
                    Expression.Constant(direction)
                )) as ITraversalQueryable<TraversalData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("Traversal_Edge")]
        public static ITraversalQueryable<TraversalData<TVertex, TEdge>> Edge<TVertex, TEdge>(
            this ITraversalQueryable<TraversalData<TVertex, TEdge>> source, string collectionName)
        {
            return source.Provider.CreateQuery<TraversalData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("Traversal_Edge", typeof(TVertex), typeof(TEdge)),
                    source.Expression,
                    Expression.Constant(collectionName)
                )) as ITraversalQueryable<TraversalData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("Traversal_Depth")]
        public static ITraversalQueryable<TraversalData<TVertex, TEdge>> Depth<TVertex, TEdge>(
            this ITraversalQueryable<TraversalData<TVertex, TEdge>> source, int min, int max)
        {
            return source.Provider.CreateQuery<TraversalData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("Traversal_Depth", typeof(TVertex), typeof(TEdge)),
                    source.Expression,
                    Expression.Constant(min),
                    Expression.Constant(max))) as ITraversalQueryable<TraversalData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("Traversal_InBound")]
        public static ITraversalQueryable<TraversalData<TVertex, TEdge>> InBound<TVertex, TEdge>(
            this ITraversalQueryable<TraversalData<TVertex, TEdge>> source)
        {
            return source.Provider.CreateQuery<TraversalData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("Traversal_InBound", typeof(TVertex), typeof(TEdge)),
                    source.Expression)) as ITraversalQueryable<TraversalData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("Traversal_OutBound")]
        public static ITraversalQueryable<TraversalData<TVertex, TEdge>> OutBound<TVertex, TEdge>(
            this ITraversalQueryable<TraversalData<TVertex, TEdge>> source)
        {
            return source.Provider.CreateQuery<TraversalData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("Traversal_OutBound", typeof(TVertex), typeof(TEdge)),
                    source.Expression)) as ITraversalQueryable<TraversalData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("Traversal_AnyDirection")]
        public static ITraversalQueryable<TraversalData<TVertex, TEdge>> AnyDirection<TVertex, TEdge>(
            this ITraversalQueryable<TraversalData<TVertex, TEdge>> source)
        {
            return source.Provider.CreateQuery<TraversalData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("Traversal_AnyDirection", typeof(TVertex), typeof(TEdge)),
                    source.Expression)) as ITraversalQueryable<TraversalData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("Traversal_Options")]
        public static ITraversalQueryable<TraversalData<TVertex, TEdge>> Options<TVertex, TEdge>(
            this ITraversalQueryable<TraversalData<TVertex, TEdge>> source, object options)
        {
            return source.Provider.CreateQuery<TraversalData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("Traversal_Options", typeof(TVertex), typeof(TEdge)),
                    source.Expression,
                    Expression.Constant(options))) as ITraversalQueryable<TraversalData<TVertex, TEdge>>;
        }
    }
}