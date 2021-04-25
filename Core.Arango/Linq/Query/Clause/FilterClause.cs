using System;
using System.Linq.Expressions;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Clauses;

namespace Core.Arango.Linq.Query.Clause
{
    internal sealed class FilterClause : IBodyClause
    {
        private Expression _predicate;

        public FilterClause(Expression predicate)
        {
            LinqUtility.CheckNotNull("predicate", predicate);
            _predicate = predicate;
        }

        public Expression Predicate
        {
            get => _predicate;
            set => _predicate = LinqUtility.CheckNotNull("value", value);
        }

        public void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
        {
            LinqUtility.CheckNotNull("visitor", visitor);
            LinqUtility.CheckNotNull("queryModel", queryModel);

            var visotor = visitor as ArangoModelVisitor;

            if (visotor == null)
                throw new Exception("QueryModelVisitor should be type of ArangoModelVisitor");

            visotor.VisitFilterClause(this, queryModel, index);
        }

        public void TransformExpressions(Func<Expression, Expression> transformation)
        {
            LinqUtility.CheckNotNull("transformation", transformation);
            Predicate = transformation(Predicate);
        }

        IBodyClause IBodyClause.Clone(CloneContext cloneContext)
        {
            return Clone(cloneContext);
        }

        public FilterClause Clone(CloneContext cloneContext)
        {
            LinqUtility.CheckNotNull("cloneContext", cloneContext);

            var clone = new FilterClause(Predicate);
            return clone;
        }

        public override string ToString()
        {
            return "filter " + Predicate;
        }
    }
}