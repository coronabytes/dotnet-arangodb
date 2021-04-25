using System;
using System.Linq.Expressions;
using System.Reflection;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Parsing.Structure.IntermediateModel;

namespace Core.Arango.Linq.Query.Clause
{
    internal class UpsertExpressionNode : MethodCallExpressionNodeBase
    {
        public static readonly MethodInfo[] SupportedMethods =
        {
            LinqUtility.GetSupportedMethod(() =>
                ArangoQueryableExtensions.InternalUpsert<object, object>(null, o => null, o => null, (o, old) => null, null))
        };

        private readonly ResolvedExpressionCache<Expression> _cachedInsertSelector;

        private readonly ResolvedExpressionCache<Expression> _cachedSearchSelector;
        private readonly ResolvedExpressionCache<Expression> _cachedUpdateSelector;

        public UpsertExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression searchSelector
            , LambdaExpression insertSelector, LambdaExpression updateSelector, ConstantExpression type)
            : base(parseInfo)
        {
            LinqUtility.CheckNotNull("searchSelector", searchSelector);
            LinqUtility.CheckNotNull("insertSelector", insertSelector);
            LinqUtility.CheckNotNull("updateSelector", updateSelector);
            LinqUtility.CheckNotNull("type", type);

            SearchSelector = searchSelector;
            InsertSelector = insertSelector;
            UpdateSelector = updateSelector;

            UpdateType = type;

            _cachedSearchSelector = new ResolvedExpressionCache<Expression>(this);
            _cachedInsertSelector = new ResolvedExpressionCache<Expression>(this);
            _cachedUpdateSelector = new ResolvedExpressionCache<Expression>(this);
        }

        public LambdaExpression SearchSelector { get; }
        public LambdaExpression InsertSelector { get; }
        public LambdaExpression UpdateSelector { get; }
        public ConstantExpression UpdateType { get; }

        public Expression GetResolvedSearchPredicate(ClauseGenerationContext clauseGenerationContext)
        {
            return _cachedSearchSelector.GetOrCreate(r =>
                r.GetResolvedExpression(SearchSelector.Body, SearchSelector.Parameters[0], clauseGenerationContext));
        }

        public Expression GetResolvedInsertPredicate(ClauseGenerationContext clauseGenerationContext)
        {
            return _cachedInsertSelector.GetOrCreate(r =>
                r.GetResolvedExpression(InsertSelector.Body, InsertSelector.Parameters[0], clauseGenerationContext));
        }

        public Expression GetResolvedUpdatePredicate(ClauseGenerationContext clauseGenerationContext)
        {
            return _cachedUpdateSelector.GetOrCreate(r =>
                r.GetResolvedExpression(UpdateSelector.Body, UpdateSelector.Parameters[0], clauseGenerationContext));
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

            queryModel.BodyClauses.Add(new UpsertClause(
                GetResolvedSearchPredicate(clauseGenerationContext),
                GetResolvedInsertPredicate(clauseGenerationContext),
                GetResolvedUpdatePredicate(clauseGenerationContext),
                queryModel.MainFromClause.ItemName,
                UpdateType.Value as Type
            ));
        }
    }
}