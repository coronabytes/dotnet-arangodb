using System;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Parsing.ExpressionVisitors;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace Core.Arango.Linq.Query.Clause
{
    internal class SelectExpressionNode : MethodCallExpressionNodeBase
    {
        private const int c_indexSelectorParameterPosition = 1;

        // no direct use of SelectExpressionNode
        //public static IEnumerable<MethodInfo> GetSupportedMethods()
        //{
        //    return ReflectionUtility.EnumerableAndQueryableMethods.WhereNameMatches("Select").WithoutIndexSelector(c_indexSelectorParameterPosition);
        //}

        private readonly ResolvedExpressionCache<Expression> _cachedSelector;

        public SelectExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression selector)
            : base(parseInfo)
        {
            LinqUtility.CheckNotNull("selector", selector);

            if (selector.Parameters.Count != 1)
                throw new ArgumentException("Selector must have exactly one parameter.", "selector");

            Selector = selector;
            _cachedSelector = new ResolvedExpressionCache<Expression>(this);
        }

        public LambdaExpression Selector { get; }

        public Expression GetResolvedSelector(ClauseGenerationContext clauseGenerationContext)
        {
            return _cachedSelector.GetOrCreate(r =>
                r.GetResolvedExpression(Selector.Body, Selector.Parameters[0], clauseGenerationContext));
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