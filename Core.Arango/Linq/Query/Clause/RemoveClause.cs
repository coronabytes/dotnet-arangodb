using System;
using System.Linq.Expressions;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Clauses;

namespace Core.Arango.Linq.Query.Clause
{
    internal class RemoveClause : IBodyClause, IModificationClause
    {
        public RemoveClause(string itemName, Type collectionType, Expression keySelector)
        {
            KeySelector = keySelector;

            ItemName = itemName;

            CollectionType = collectionType;
        }

        public Expression KeySelector { get; set; }

        public virtual void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
        {
            LinqUtility.CheckNotNull("visitor", visitor);
            LinqUtility.CheckNotNull("queryModel", queryModel);

            var arangoVisitor = visitor as ArangoModelVisitor;

            if (arangoVisitor == null)
                throw new Exception("QueryModelVisitor should be type of ArangoModelVisitor");

            arangoVisitor.VisitRemoveClause(this, queryModel);
        }

        public virtual void TransformExpressions(Func<Expression, Expression> transformation)
        {
            LinqUtility.CheckNotNull("transformation", transformation);
            KeySelector = transformation(KeySelector);
        }

        IBodyClause IBodyClause.Clone(CloneContext cloneContext)
        {
            return Clone(cloneContext);
        }

        public string ItemName { get; set; }

        public Type CollectionType { get; set; }

        public bool IgnoreSelect { get; set; }

        public RemoveClause Clone(CloneContext cloneContext)
        {
            LinqUtility.CheckNotNull("cloneContext", cloneContext);

            var result = new RemoveClause(ItemName, CollectionType, KeySelector);
            return result;
        }
    }
}