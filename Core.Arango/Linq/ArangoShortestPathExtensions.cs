using System;
using System.Linq;
using System.Linq.Expressions;
using Core.Arango.Linq.Collection;
using Core.Arango.Linq.Data;
using Core.Arango.Linq.Interface;
using Core.Arango.Linq.Query;

namespace Core.Arango.Linq
{
    public static class ArangoShortestPathExtensions
    {
        [ExtentionIdentifier("ShortestPath_Selector")]
        public static IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> ShortestPath<TVertex, TEdge>(
            this IQueryable source, Expression<Func<string>> startVertex, Expression<Func<string>> targetVertex)
        {
            return source.Provider.CreateQuery<ShortestPathData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("ShortestPath_Selector", typeof(TVertex), typeof(TEdge)),
                    source.Expression,
                    Expression.Quote(startVertex),
                    Expression.Quote(targetVertex)
                )) as IShortestPathQueryable<ShortestPathData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("ShortestPath_Constant")]
        public static IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> ShortestPath<TVertex, TEdge>(
            this IQueryable source, string startVertex, string targetVertex)
        {
            return source.Provider.CreateQuery<ShortestPathData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("ShortestPath_Constant", typeof(TVertex), typeof(TEdge)),
                    source.Expression,
                    Expression.Constant(startVertex),
                    Expression.Constant(targetVertex)
                )) as IShortestPathQueryable<ShortestPathData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("ShortestPath_Graph")]
        public static IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> Graph<TVertex, TEdge>(
            this IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> source, string graphName)
        {
            return source.Provider.CreateQuery<ShortestPathData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("ShortestPath_Graph", typeof(TVertex), typeof(TEdge)),
                    source.Expression,
                    Expression.Constant(graphName)
                )) as IShortestPathQueryable<ShortestPathData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("ShortestPath_Edge")]
        public static IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> Edge<TVertex, TEdge>(
            this IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> source, string collectionName)
        {
            return source.Provider.CreateQuery<ShortestPathData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("ShortestPath_Edge", typeof(TVertex), typeof(TEdge)),
                    source.Expression,
                    Expression.Constant(collectionName)
                )) as IShortestPathQueryable<ShortestPathData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("ShortestPath_Edge_Direction")]
        public static IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> Edge<TVertex, TEdge>(
            this IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> source, string collectionName,
            EdgeDirection direction)
        {
            return source.Provider.CreateQuery<ShortestPathData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("ShortestPath_Edge_Direction", typeof(TVertex), typeof(TEdge)),
                    source.Expression,
                    Expression.Constant(collectionName),
                    Expression.Constant(direction)
                )) as IShortestPathQueryable<ShortestPathData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("ShortestPath_InBound")]
        public static IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> InBound<TVertex, TEdge>(
            this IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> source)
        {
            return source.Provider.CreateQuery<ShortestPathData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("ShortestPath_InBound", typeof(TVertex), typeof(TEdge)),
                    source.Expression)) as IShortestPathQueryable<ShortestPathData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("ShortestPath_OutBound")]
        public static IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> OutBound<TVertex, TEdge>(
            this IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> source)
        {
            return source.Provider.CreateQuery<ShortestPathData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("ShortestPath_OutBound", typeof(TVertex), typeof(TEdge)),
                    source.Expression)) as IShortestPathQueryable<ShortestPathData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("ShortestPath_AnyDirection")]
        public static IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> AnyDirection<TVertex, TEdge>(
            this IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> source)
        {
            return source.Provider.CreateQuery<ShortestPathData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("ShortestPath_AnyDirection", typeof(TVertex), typeof(TEdge)),
                    source.Expression)) as IShortestPathQueryable<ShortestPathData<TVertex, TEdge>>;
        }

        [ExtentionIdentifier("ShortestPath_Options")]
        public static IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> Options<TVertex, TEdge>(
            this IShortestPathQueryable<ShortestPathData<TVertex, TEdge>> source, object options)
        {
            return source.Provider.CreateQuery<ShortestPathData<TVertex, TEdge>>(
                Expression.Call(
                    ArangoQueryableExtensions.FindExtention("ShortestPath_Options", typeof(TVertex), typeof(TEdge)),
                    source.Expression,
                    Expression.Constant(options)
                )) as IShortestPathQueryable<ShortestPathData<TVertex, TEdge>>;
        }
    }
}