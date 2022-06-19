using System;
using System.Linq.Expressions;
using System.Reflection;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Parsing.Structure.IntermediateModel;

namespace Core.Arango.Linq.Query.Clause
{
    internal class PartialUpdateExpressionNode : MethodCallExpressionNodeBase
    {
        public static readonly MethodInfo[] SupportedMethods =
        {
            LinqUtility.GetSupportedMethod(() =>
                ArangoQueryableExtensions.PartialUpdate<object>(null, o => null, o => null, o => null))
        };

        private readonly ResolvedExpressionCache<Expression> _cachedKeySelector;

        private readonly ResolvedExpressionCache<Expression> _cachedUpdateFieldSelector;

        private readonly ResolvedExpressionCache<Expression> _cachedWithSelector;

        public PartialUpdateExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression updateFieldSelector, LambdaExpression withSelector
            , LambdaExpression keySelector, ConstantExpression command)
            : base(parseInfo)
        {
            LinqUtility.CheckNotNull("withSelector", withSelector);

            if (withSelector.Parameters.Count != 1)
                throw new ArgumentException("WithSelector must have exactly one parameter.", "withSelector");

            WithSelector = withSelector;
            UpdateFieldSelector = updateFieldSelector;

            _cachedWithSelector = new ResolvedExpressionCache<Expression>(this);
            _cachedUpdateFieldSelector = new ResolvedExpressionCache<Expression>(this);

            var keyConstant = keySelector.Body as ConstantExpression;
            if (keyConstant == null || keyConstant.Value != null)
            {
                KeySelector = keySelector;
                _cachedKeySelector = new ResolvedExpressionCache<Expression>(this);
            }

            Command = command;
        }

        public LambdaExpression WithSelector { get; }
        public LambdaExpression KeySelector { get; }
        public LambdaExpression UpdateFieldSelector { get; }

        public ConstantExpression Command { get; }

        public Expression GetResolvedPredicate(ClauseGenerationContext clauseGenerationContext)
        {
            return _cachedWithSelector.GetOrCreate(r =>
                r.GetResolvedExpression(WithSelector.Body, WithSelector.Parameters[0], clauseGenerationContext));
        }

        public Expression GetResolvedKeyPredicate(ClauseGenerationContext clauseGenerationContext)
        {
            return _cachedKeySelector.GetOrCreate(r =>
                r.GetResolvedExpression(KeySelector.Body, KeySelector.Parameters[0], clauseGenerationContext));
        }

        public Expression GetResolvedUpdateFieldPredicate(ClauseGenerationContext clauseGenerationContext)
        {
            return _cachedUpdateFieldSelector.GetOrCreate(r =>
                r.GetResolvedExpression(UpdateFieldSelector.Body, UpdateFieldSelector.Parameters[0], clauseGenerationContext));
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

            queryModel.BodyClauses.Add(new PartialUpdateClause(
                UpdateFieldSelector != null ? GetResolvedUpdateFieldPredicate(clauseGenerationContext) : null,
                GetResolvedPredicate(clauseGenerationContext),
                queryModel.MainFromClause.ItemName,
                WithSelector.Parameters[0].Type,
                KeySelector != null ? GetResolvedKeyPredicate(clauseGenerationContext) : null
                ));
        }
    }
}