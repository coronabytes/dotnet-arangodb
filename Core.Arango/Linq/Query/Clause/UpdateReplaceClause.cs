using System;
using System.Linq.Expressions;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Clauses;

namespace Core.Arango.Linq.Query.Clause
{
    internal class UpdateReplaceClause : IBodyClause, IModificationClause
    {
        public UpdateReplaceClause(Expression withSelector, string itemName, Type collectionType,
            Expression keySelector, string command)
        {
            LinqUtility.CheckNotNull("selector", withSelector);

            WithSelector = withSelector;
            KeySelector = keySelector;

            ItemName = itemName;

            CollectionType = collectionType;

            Command = command;
        }

        public string Command { get; set; }


        public Expression WithSelector { get; set; }

        public Expression KeySelector { get; set; }

        public virtual void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
        {
            LinqUtility.CheckNotNull("visitor", visitor);
            LinqUtility.CheckNotNull("queryModel", queryModel);

            var arangoVisitor = visitor as ArangoModelVisitor;

            if (arangoVisitor == null)
                throw new Exception("QueryModelVisitor should be type of ArangoModelVisitor");

            arangoVisitor.VisitUpdateReplaceClause(this, queryModel);
        }

        public virtual void TransformExpressions(Func<Expression, Expression> transformation)
        {
            LinqUtility.CheckNotNull("transformation", transformation);
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

        public UpdateReplaceClause Clone(CloneContext cloneContext)
        {
            LinqUtility.CheckNotNull("cloneContext", cloneContext);

            var result = new UpdateReplaceClause(WithSelector, ItemName, CollectionType, KeySelector, Command);
            return result;
        }
    }
}