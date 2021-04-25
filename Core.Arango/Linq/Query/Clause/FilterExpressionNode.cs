using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Parsing.Structure.IntermediateModel;

namespace Core.Arango.Linq.Query.Clause
{
    internal class FilterExpressionNode : MethodCallExpressionNodeBase
    {
        private const int c_indexSelectorParameterPosition = 1;

        public static IEnumerable<MethodInfo> GetSupportedMethods = SupportedMethodSpecifications
            .EnumerableAndQueryableMethods
            .WhereNameMatches("Filter")
            .WithoutIndexSelector(c_indexSelectorParameterPosition);

        private readonly ResolvedExpressionCache<Expression> _cachedPredicate;

        public FilterExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression predicate)
            : base(parseInfo)
        {
            LinqUtility.CheckNotNull("predicate", predicate);

            if (predicate.Parameters.Count != 1)
                throw new ArgumentException("Predicate must have exactly one parameter.", "predicate");

            Predicate = predicate;
            _cachedPredicate = new ResolvedExpressionCache<Expression>(this);
        }

        public LambdaExpression Predicate { get; }

        public Expression GetResolvedPredicate(ClauseGenerationContext clauseGenerationContext)
        {
            return _cachedPredicate.GetOrCreate(r =>
                r.GetResolvedExpression(Predicate.Body, Predicate.Parameters[0], clauseGenerationContext));
        }

        public override Expression Resolve(
            ParameterExpression inputParameter, Expression expressionToBeResolved,
            ClauseGenerationContext clauseGenerationContext)
        {
            LinqUtility.CheckNotNull("inputParameter", inputParameter);
            LinqUtility.CheckNotNull("expressionToBeResolved", expressionToBeResolved);

            // this simply streams its input data to the output without modifying its structure, so we resolve by passing on the data to the previous node
            return Source.Resolve(inputParameter, expressionToBeResolved, clauseGenerationContext);
        }

        protected override void ApplyNodeSpecificSemantics(QueryModel queryModel,
            ClauseGenerationContext clauseGenerationContext)
        {
            LinqUtility.CheckNotNull("queryModel", queryModel);

            var clause = new FilterClause(GetResolvedPredicate(clauseGenerationContext));
            queryModel.BodyClauses.Add(clause);
        }
    }
}