using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Arango.Linq.Collection;
using Core.Arango.Linq.Utility;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Parsing.Structure.IntermediateModel;

namespace Core.Arango.Linq.Query.Clause
{
    internal class TraversalDirectionExpressionNode : MethodCallExpressionNodeBase
    {
        public static readonly MethodInfo[] SupportedMethods =
        {
            LinqUtility.GetSupportedMethod(() => TraversalQueryableExtensions.InBound<object, object>(null)),
            LinqUtility.GetSupportedMethod(() => TraversalQueryableExtensions.OutBound<object, object>(null)),
            LinqUtility.GetSupportedMethod(() => TraversalQueryableExtensions.AnyDirection<object, object>(null)),
            LinqUtility.GetSupportedMethod(() => ShortestPathQueryableExtensions.InBound<object, object>(null)),
            LinqUtility.GetSupportedMethod(() => ShortestPathQueryableExtensions.OutBound<object, object>(null)),
            LinqUtility.GetSupportedMethod(() => ShortestPathQueryableExtensions.AnyDirection<object, object>(null))
        };

        public TraversalDirectionExpressionNode(MethodCallExpressionParseInfo parseInfo,
            ConstantExpression direction)
            : base(parseInfo)
        {
            switch (parseInfo.ParsedExpression.Method.Name)
            {
                case nameof(TraversalQueryableExtensions.InBound):
                    Direction = Expression.Constant(Utils.EdgeDirectionToString(EdgeDirection.Inbound));
                    break;
                case nameof(TraversalQueryableExtensions.OutBound):
                    Direction = Expression.Constant(Utils.EdgeDirectionToString(EdgeDirection.Outbound));
                    break;
                case nameof(TraversalQueryableExtensions.AnyDirection):
                    Direction = Expression.Constant(Utils.EdgeDirectionToString(EdgeDirection.Any));
                    break;
            }
        }

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

            traversalClause.Direction = Direction;
        }
    }
}