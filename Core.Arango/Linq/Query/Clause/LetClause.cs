using System;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Clauses;

namespace Core.Arango.Linq.Query.Clause
{
    internal sealed class LetClause : IBodyClause, IQuerySource
    {
        private Expression _letExpression;

        public LetClause(string itemName, Expression letExpression)
        {
            LinqUtility.CheckNotNull("itemName", itemName);
            LinqUtility.CheckNotNull("letExpression", letExpression);

            ItemName = itemName;
            _letExpression = letExpression;
        }

        public LetClause(string itemName, Expression letExpression, Expression subQueryexpression)
            : this(itemName, letExpression)
        {
            SubqueryExpression = subQueryexpression;
        }

        public Expression LetExpression
        {
            get => _letExpression;
            set => _letExpression = LinqUtility.CheckNotNull("value", value);
        }

        public Expression SubqueryExpression { get; set; }

        public void TransformExpressions(Func<Expression, Expression> transformation)
        {
            _letExpression = transformation(_letExpression);
        }

        public void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
        {
            var arangoVisotor = visitor as ArangoModelVisitor;
            if (arangoVisotor == null)
                throw new Exception("QueryModelVisitor should be type of ArangoModelVisitor");

            arangoVisotor.VisitLetClause(this, queryModel, typeof(object));
        }

        public IBodyClause Clone(CloneContext cloneContext)
        {
            return new LetClause(ItemName, LetExpression);
        }

        public string ItemName { get; }

        public Type ItemType => LetExpression.Type;

        public override string ToString()
        {
            return string.Format("let {0} = {1}", ItemName, LetExpression);
        }
    }
}