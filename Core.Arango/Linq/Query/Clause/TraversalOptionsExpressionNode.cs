using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Parsing.Structure.IntermediateModel;

namespace Core.Arango.Linq.Query.Clause
{
    internal class TraversalOptionsExpressionNode : MethodCallExpressionNodeBase
    {
        public static readonly MethodInfo[] SupportedMethods =
        {
            LinqUtility.GetSupportedMethod(() => TraversalQueryableExtensions.Options<object, object>(null, null)),
            LinqUtility.GetSupportedMethod(() => ShortestPathQueryableExtensions.Options<object, object>(null, null))
        };

        public TraversalOptionsExpressionNode(MethodCallExpressionParseInfo parseInfo, ConstantExpression options)
            : base(parseInfo)
        {
            Options = options;
        }

        public ConstantExpression Options { get; }

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

            traversalClause.Options = Options;
        }
    }
}