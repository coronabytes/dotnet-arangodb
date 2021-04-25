using System;
using System.Linq.Expressions;
using Core.Arango.Relinq.Clauses;
using Core.Arango.Relinq.Utilities;
using Remotion.Utilities;

namespace Core.Arango.Relinq.Parsing.Structure.IntermediateModel
{
    /// <summary>
    ///     Acts as a base class for <see cref="UnionExpressionNode" /> and <see cref="ConcatExpressionNode" />, i.e., for node
    ///     parsers for set operations
    ///     acting as an <see cref="IQuerySource" />.
    /// </summary>
    internal abstract class QuerySourceSetOperationExpressionNodeBase : ResultOperatorExpressionNodeBase,
        IQuerySourceExpressionNode
    {
        protected QuerySourceSetOperationExpressionNodeBase(MethodCallExpressionParseInfo parseInfo, Expression source2)
            : base(parseInfo, null, null)
        {
            ArgumentUtility.CheckNotNull("source2", source2);
            Source2 = source2;
            ItemType = ReflectionUtility.GetItemTypeOfClosedGenericIEnumerable(parseInfo.ParsedExpression.Type,
                "expression");
        }

        public Expression Source2 { get; }

        public Type ItemType { get; }

        public sealed override Expression Resolve(ParameterExpression inputParameter, Expression expressionToBeResolved,
            ClauseGenerationContext clauseGenerationContext)
        {
            ArgumentUtility.CheckNotNull("inputParameter", inputParameter);
            ArgumentUtility.CheckNotNull("expressionToBeResolved", expressionToBeResolved);

            // UnionResultOperator is a query source, so expressions resolve their input parameter with the UnionResultOperator created by this node.
            return QuerySourceExpressionNodeUtility.ReplaceParameterWithReference(this, inputParameter,
                expressionToBeResolved, clauseGenerationContext);
        }

        protected abstract ResultOperatorBase CreateSpecificResultOperator();

        protected sealed override ResultOperatorBase CreateResultOperator(
            ClauseGenerationContext clauseGenerationContext)
        {
            var resultOperator = CreateSpecificResultOperator();
            clauseGenerationContext.AddContextInfo(this, resultOperator);
            return resultOperator;
        }
    }
}