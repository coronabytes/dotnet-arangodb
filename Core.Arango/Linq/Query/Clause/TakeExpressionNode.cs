using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Parsing.Structure.IntermediateModel;

namespace Core.Arango.Linq.Query.Clause
{
    internal class TakeExpressionNode : MethodCallExpressionNodeBase
    {
        public static readonly MethodInfo[] SupportedMethods =
        {
            LinqUtility.GetSupportedMethod(() => Queryable.Take<object>(null, 0)),
            LinqUtility.GetSupportedMethod(() => Enumerable.Take<object>(null, 0))
        };

        public TakeExpressionNode(MethodCallExpressionParseInfo parseInfo, Expression count)
            : base(parseInfo)
        {
            Count = count;
        }

        public Expression Count { get; }


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

            var lastClause = queryModel.BodyClauses.LastOrDefault();
            var skipTakeClause = lastClause as SkipTakeClause;
            if (skipTakeClause != null)
                skipTakeClause.TakeCount = Count;
            else
                queryModel.BodyClauses.Add(new SkipTakeClause(null, Count));
        }
    }
}