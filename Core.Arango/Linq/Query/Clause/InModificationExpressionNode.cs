using System;
using System.Linq.Expressions;
using System.Reflection;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Parsing.Structure.IntermediateModel;

namespace Core.Arango.Linq.Query.Clause
{
    internal class InModificationExpressionNode : MethodCallExpressionNodeBase
    {
        public static readonly MethodInfo[] SupportedMethods =
        {
            LinqUtility.GetSupportedMethod(() => ArangoQueryableExtensions.In<object>(null))
        };

        public Type CollectionToModify;

        public InModificationExpressionNode(MethodCallExpressionParseInfo parseInfo)
            : base(parseInfo)
        {
            CollectionToModify = parseInfo.ParsedExpression.Type.GenericTypeArguments[0];
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

            var modificationClause = queryModel.BodyClauses.NextBodyClause<IModificationClause>();
            modificationClause.CollectionType = CollectionToModify;
        }
    }
}