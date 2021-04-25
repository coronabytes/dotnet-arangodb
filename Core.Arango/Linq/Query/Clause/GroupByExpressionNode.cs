using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Arango.Linq.Utility;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Parsing.ExpressionVisitors;
using Core.Arango.Relinq.Parsing.Structure.IntermediateModel;

namespace Core.Arango.Linq.Query.Clause
{
    internal class GroupByExpressionNode : SelectExpressionNode, IQuerySourceExpressionNode
    {
        public static IEnumerable<MethodInfo> GetSupportedMethods = SupportedMethodSpecifications
            .EnumerableAndQueryableMethods.WhereNameMatches("GroupBy").WithoutResultSelector()
            .WithoutEqualityComparer();

        private readonly ResolvedExpressionCache<Expression> _resolvedAdaptedSelector;

        private readonly string intoIdentifier;

        public GroupByExpressionNode(MethodCallExpressionParseInfo parseInfo, LambdaExpression selector)
            : base(Utils.CheckNotNull("parseInfo", parseInfo), Utils.CheckNotNull("selector", selector))
        {
            intoIdentifier = parseInfo.AssociatedIdentifier;
            _resolvedAdaptedSelector = new ResolvedExpressionCache<Expression>(this);
        }

        public override Expression Resolve(
            ParameterExpression inputParameter, Expression expressionToBeResolved,
            ClauseGenerationContext clauseGenerationContext)
        {
            Utils.CheckNotNull("inputParameter", inputParameter);
            Utils.CheckNotNull("expressionToBeResolved", expressionToBeResolved);

            var resolvedSelector = GetResolvedAdaptedSelector(clauseGenerationContext);
            return ReplacingExpressionVisitor.Replace(inputParameter, resolvedSelector, expressionToBeResolved);
        }

        protected override void ApplyNodeSpecificSemantics(QueryModel queryModel,
            ClauseGenerationContext clauseGenerationContext)
        {
            Utils.CheckNotNull("queryModel", queryModel);

            var groupByClause =
                new GroupByClause(GetResolvedSelector(clauseGenerationContext), Selector, intoIdentifier);
            queryModel.BodyClauses.Add(groupByClause);

            clauseGenerationContext.AddContextInfo(this, groupByClause);

            queryModel.SelectClause.Selector = GetResolvedAdaptedSelector(clauseGenerationContext);
        }

        public Expression GetResolvedAdaptedSelector(ClauseGenerationContext clauseGenerationContext)
        {
            return _resolvedAdaptedSelector.GetOrCreate(
                r =>
                {
                    var groupingType =
                        typeof(Grouping<,>).MakeGenericType(Selector.Body.Type, Selector.Parameters[0].Type);
                    var interface_groupingType =
                        typeof(IGrouping<,>).MakeGenericType(Selector.Body.Type, Selector.Parameters[0].Type);

                    var constr = ReflectionUtils.GetConstructors(groupingType).ToList()[0];
                    var parameters = constr.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name))
                        .ToArray();
                    var newExpression =
                            Expression.Convert(
                                Expression.New(constr, parameters, ReflectionUtils.GetMember(groupingType, "Key")[0])
                                , interface_groupingType)
                        ;

                    return r.GetResolvedExpression(newExpression, Selector.Parameters[0], clauseGenerationContext);
                });
        }
    }
}