using System;
using System.Linq.Expressions;
using System.Reflection;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Parsing.ExpressionVisitors;
using Core.Arango.Relinq.Parsing.Structure.IntermediateModel;

namespace Core.Arango.Linq.Query.Clause
{
    internal class SelectModificationExpressionNode : MethodCallExpressionNodeBase
    {
        public static readonly MethodInfo[] SupportedMethods =
        {
            LinqUtility.GetSupportedMethod(() => QueryableExtensions.Select<object, object>(null, (x, y) => null))
        };

        private readonly ResolvedExpressionCache<Expression> _cachedSelector;

        public SelectModificationExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression selector)
            : base(parseInfo)
        {
            LinqUtility.CheckNotNull("selector", selector);

            if (selector.Parameters.Count != 2)
                throw new ArgumentException("Selector must have exactly two parameter.", "selector");

            Selector = selector;
            _cachedSelector = new ResolvedExpressionCache<Expression>(this);
        }

        public LambdaExpression Selector { get; }

        public Expression GetResolvedSelector(ClauseGenerationContext clauseGenerationContext)
        {
            return _cachedSelector.GetOrCreate(r =>
            {
                return r.GetResolvedExpression(Selector.Body, Expression.Parameter(typeof(int), ""),
                    clauseGenerationContext);
                //return r.GetResolvedExpression(Selector.Body, Selector.Parameters[0], clauseGenerationContext);
            });
        }

        public override Expression Resolve(
            ParameterExpression inputParameter, Expression expressionToBeResolved,
            ClauseGenerationContext clauseGenerationContext)
        {
            LinqUtility.CheckNotNull("inputParameter", inputParameter);
            LinqUtility.CheckNotNull("expressionToBeResolved", expressionToBeResolved);

            // we modify the structure of the stream of data coming into this node by our selector,
            // so we first resolve the selector, then we substitute the result for the inputParameter in the expressionToBeResolved
            var resolvedSelector = GetResolvedSelector(clauseGenerationContext);
            return ReplacingExpressionVisitor.Replace(inputParameter, resolvedSelector, expressionToBeResolved);
        }

        protected override void ApplyNodeSpecificSemantics(QueryModel queryModel,
            ClauseGenerationContext clauseGenerationContext)
        {
            LinqUtility.CheckNotNull("queryModel", queryModel);
            queryModel.SelectClause.Selector = GetResolvedSelector(clauseGenerationContext);
        }
    }
}