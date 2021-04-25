using System;
using System.Linq.Expressions;
using System.Reflection;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace Core.Arango.Linq.Query.Clause
{
    internal class InsertExpressionNode : MethodCallExpressionNodeBase
    {
        public static readonly MethodInfo[] SupportedMethods =
        {
            LinqUtility.GetSupportedMethod(() => QueryableExtensions.Insert<object>(null, o => null, null))
        };

        private readonly ResolvedExpressionCache<Expression> _cachedWithSelector;

        public InsertExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression withSelector
            , ConstantExpression collection)
            : base(parseInfo)
        {
            var withConstant = withSelector.Body as ConstantExpression;
            if (withConstant == null || withConstant.Value != null)
            {
                WithSelector = withSelector;
                _cachedWithSelector = new ResolvedExpressionCache<Expression>(this);
            }

            Collection = collection;
        }

        public LambdaExpression WithSelector { get; }

        public ConstantExpression Collection { get; }


        public Expression GetResolvedPredicate(ClauseGenerationContext clauseGenerationContext)
        {
            return _cachedWithSelector.GetOrCreate(r =>
                r.GetResolvedExpression(WithSelector.Body, WithSelector.Parameters[0], clauseGenerationContext));
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

            queryModel.BodyClauses.Add(new InsertClause(
                WithSelector != null ? GetResolvedPredicate(clauseGenerationContext) : null,
                queryModel.MainFromClause.ItemName,
                (Type) Collection.Value
            ));
        }
    }
}