using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Arango.Linq.Collection;
using Core.Arango.Linq.Data;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace Core.Arango.Linq.Query.Clause
{
    internal class TraversalEdgeExpressionNode : MethodCallExpressionNodeBase, IQuerySourceExpressionNode
    {
        public static readonly MethodInfo[] SupportedMethods =
        {
            LinqUtility.GetSupportedMethod(() => TraversalQueryableExtensions.Edge<object, object>(null, null)),
            LinqUtility.GetSupportedMethod(() =>
                TraversalQueryableExtensions.Edge<object, object>(null, null, EdgeDirection.Any)),
            LinqUtility.GetSupportedMethod(() => ShortestPathQueryableExtensions.Edge<object, object>(null, null)),
            LinqUtility.GetSupportedMethod(() =>
                ShortestPathQueryableExtensions.Edge<object, object>(null, null, EdgeDirection.Any))
        };

        public TraversalEdgeExpressionNode(MethodCallExpressionParseInfo parseInfo
            , ConstantExpression collectionName
            , ConstantExpression direction)
            : base(parseInfo)
        {
            CollectionName = collectionName;
            Direction = direction;
        }

        public ConstantExpression CollectionName { get; }

        public ConstantExpression Direction { get; }

        public override Expression Resolve(ParameterExpression inputParameter, Expression expressionToBeResolved,
            ClauseGenerationContext clauseGenerationContext)
        {
            LinqUtility.CheckNotNull("inputParameter", inputParameter);
            LinqUtility.CheckNotNull("expressionToBeResolved", expressionToBeResolved);

            return Source.Resolve(inputParameter, expressionToBeResolved, clauseGenerationContext);
        }

        protected override void ApplyNodeSpecificSemantics(QueryModel queryModel,
            ClauseGenerationContext clauseGenerationContext)
        {
            LinqUtility.CheckNotNull("queryModel", queryModel);

            var traversalClause = queryModel.BodyClauses.Last(b => b is ITraversalClause) as ITraversalClause;

            traversalClause.EdgeCollections.Add(new TraversalEdgeDefinition
            {
                CollectionName = CollectionName.Value.ToString(),
                Direction = Direction != null ? Direction.Value as EdgeDirection? : null
            });
        }
    }
}