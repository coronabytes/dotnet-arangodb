using System;
using System.Linq.Expressions;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Clauses;

namespace Core.Arango.Linq.Query.Clause
{
    internal class OptionsClause : IBodyClause
    {
        public OptionsClause(Expression options)
        {
            Options = options;
        }

        public Expression Options { get; set; }

        public virtual void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
        {
            LinqUtility.CheckNotNull("visitor", visitor);
            LinqUtility.CheckNotNull("queryModel", queryModel);

            var arangoVisitor = visitor as ArangoModelVisitor;

            if (arangoVisitor == null)
                throw new Exception("QueryModelVisitor should be type of ArangoModelVisitor");

            arangoVisitor.VisitOptionsClause(this, queryModel);
        }

        public virtual void TransformExpressions(Func<Expression, Expression> transformation)
        {
            LinqUtility.CheckNotNull("transformation", transformation);
            Options = transformation(Options);
        }

        IBodyClause IBodyClause.Clone(CloneContext cloneContext)
        {
            return Clone(cloneContext);
        }

        public OptionsClause Clone(CloneContext cloneContext)
        {
            LinqUtility.CheckNotNull("cloneContext", cloneContext);

            var result = new OptionsClause(Options);
            return result;
        }
    }
}