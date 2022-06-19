using System;
using System.Linq.Expressions;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Clauses;

namespace Core.Arango.Linq.Query.Clause
{
    internal class PartialUpdateClause : IBodyClause, IModificationClause
    {
        public PartialUpdateClause(Expression updateFieldSelector, Expression withSelector, string itemName, Type collectionType,
            Expression keySelector)
        {
            LinqUtility.CheckNotNull("selector", withSelector);

            LinqUtility.CheckNotNull("selector", updateFieldSelector);

            WithSelector = withSelector;
            UpdateFieldSelector = updateFieldSelector;
            KeySelector = keySelector;

            ItemName = itemName;

            CollectionType = collectionType;

        }

        public Expression WithSelector { get; set; }

        public Expression UpdateFieldSelector { get; set; }

        public Expression KeySelector { get; set; }

        public virtual void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
        {
            LinqUtility.CheckNotNull("visitor", visitor);
            LinqUtility.CheckNotNull("queryModel", queryModel);

            var arangoVisitor = visitor as ArangoModelVisitor;

            if (arangoVisitor == null)
                throw new Exception("QueryModelVisitor should be type of ArangoModelVisitor");

            arangoVisitor.VisitPartialUpdateClause(this, queryModel);
        }

        public virtual void TransformExpressions(Func<Expression, Expression> transformation)
        {
            LinqUtility.CheckNotNull("transformation", transformation);
            UpdateFieldSelector = transformation(UpdateFieldSelector);
            WithSelector = transformation(WithSelector);
            KeySelector = transformation(KeySelector);
        }

        IBodyClause IBodyClause.Clone(CloneContext cloneContext)
        {
            return Clone(cloneContext);
        }

        public string ItemName { get; set; }

        public Type CollectionType { get; set; }

        public bool IgnoreSelect { get; set; }

        public PartialUpdateClause Clone(CloneContext cloneContext)
        {
            LinqUtility.CheckNotNull("cloneContext", cloneContext);

            var result = new PartialUpdateClause(UpdateFieldSelector, WithSelector, ItemName, CollectionType, KeySelector);
            return result;
        }
    }
}