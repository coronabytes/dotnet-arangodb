using System;
using System.Linq.Expressions;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Clauses;

namespace Core.Arango.Linq.Query.Clause
{
    internal class UpsertClause : IBodyClause, IModificationClause
    {
        public UpsertClause(Expression searchSelector, Expression insertSelector, Expression updateSelector,
            string itemName, Type collectionType)
        {
            SearchSelector = searchSelector;
            InsertSelector = insertSelector;
            UpdateSelector = updateSelector;

            ItemName = itemName;

            CollectionType = collectionType;
        }

        public Expression SearchSelector { get; set; }

        public Expression InsertSelector { get; set; }

        public Expression UpdateSelector { get; set; }

        public virtual void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
        {
            LinqUtility.CheckNotNull("visitor", visitor);
            LinqUtility.CheckNotNull("queryModel", queryModel);

            var arangoVisitor = visitor as ArangoModelVisitor;

            if (arangoVisitor == null)
                throw new Exception("QueryModelVisitor should be type of ArangoModelVisitor");

            arangoVisitor.VisitUpsertClause(this, queryModel);
        }

        public virtual void TransformExpressions(Func<Expression, Expression> transformation)
        {
            LinqUtility.CheckNotNull("transformation", transformation);
            SearchSelector = transformation(SearchSelector);
            InsertSelector = transformation(InsertSelector);
            UpdateSelector = transformation(UpdateSelector);
        }

        IBodyClause IBodyClause.Clone(CloneContext cloneContext)
        {
            return Clone(cloneContext);
        }

        public string ItemName { get; set; }

        public Type CollectionType { get; set; }

        public bool IgnoreSelect { get; set; }

        public UpsertClause Clone(CloneContext cloneContext)
        {
            LinqUtility.CheckNotNull("cloneContext", cloneContext);

            var result = new UpsertClause(SearchSelector, InsertSelector, UpdateSelector, ItemName, CollectionType);
            return result;
        }
    }
}