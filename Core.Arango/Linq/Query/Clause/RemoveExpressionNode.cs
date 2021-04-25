using System;
using System.Linq.Expressions;
using System.Reflection;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace Core.Arango.Linq.Query.Clause
{
    internal class RemoveExpressionNode : MethodCallExpressionNodeBase
    {
        public static readonly MethodInfo[] SupportedMethods =
        {
            LinqUtility.GetSupportedMethod(() => QueryableExtensions.Remove<object>(null, o => null, null))
        };

        private readonly ResolvedExpressionCache<Expression> _cachedKeySelector;

        public RemoveExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression keySelector
            , ConstantExpression collection)
            : base(parseInfo)
        {
            var keyConstant = keySelector.Body as ConstantExpression;
            if (keyConstant == null || keyConstant.Value != null)
            {
                KeySelector = keySelector;
                _cachedKeySelector = new ResolvedExpressionCache<Expression>(this);
            }

            Collection = collection;
        }

        public LambdaExpression KeySelector { get; }

        public ConstantExpression Collection { get; }

        public Expression GetResolvedKeyPredicate(ClauseGenerationContext clauseGenerationContext)
        {
            return _cachedKeySelector.GetOrCreate(r =>
                r.GetResolvedExpression(KeySelector.Body, KeySelector.Parameters[0], clauseGenerationContext));
        }

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

            queryModel.BodyClauses.Add(new RemoveClause(
                queryModel.MainFromClause.ItemName,
                (Type) Collection.Value,
                KeySelector != null ? GetResolvedKeyPredicate(clauseGenerationContext) : null));
        }
    }
}