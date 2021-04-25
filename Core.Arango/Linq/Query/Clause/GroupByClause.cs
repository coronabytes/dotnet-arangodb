using System;
using System.Linq.Expressions;
using Core.Arango.Relinq;
using Core.Arango.Relinq.Clauses;
using Core.Arango.Relinq.Clauses.Expressions;

namespace Core.Arango.Linq.Query.Clause
{
    internal sealed class GroupByClause : IBodyClause
    {
        public readonly bool GroupOnLastGroup;

        private readonly string intoIdentifier;

        private readonly LambdaExpression lambdaSelector;
        private Expression _selector;

        public string FromParameterName;

        public Func<string, string> FuncIntoName;

        public string IntoName;

        public GroupByClause(Expression selector, LambdaExpression lambdaSelector, string intoIdentifier)
        {
            LinqUtility.CheckNotNull("selector", selector);

            var memberExoression = selector as MemberExpression;
            if (memberExoression != null &&
                ((memberExoression.Expression as QuerySourceReferenceExpression)
                    .ReferencedQuerySource as MainFromClause).FromExpression.Type.Name == "IGrouping`2")
                GroupOnLastGroup = true;

            this.intoIdentifier = intoIdentifier;

            this.lambdaSelector = lambdaSelector;

            Visited = false;

            _selector = selector;
        }

        public bool Visited { get; set; }

        public string CollectVariableName { get; set; }

        public Expression Selector
        {
            get => _selector;
            set => _selector = LinqUtility.CheckNotNull("value", value);
        }

        public void Accept(IQueryModelVisitor visitor, QueryModel queryModel, int index)
        {
            LinqUtility.CheckNotNull("visitor", visitor);
            LinqUtility.CheckNotNull("queryModel", queryModel);

            var arangoVisitor = visitor as ArangoModelVisitor;

            if (arangoVisitor == null)
                throw new Exception("QueryModelVisitor should be type of AqlModelVisitor");

            arangoVisitor.VisitGroupByClause(this, queryModel, index);
        }

        public void TransformExpressions(Func<Expression, Expression> transformation)
        {
            LinqUtility.CheckNotNull("transformation", transformation);
            Selector = transformation(Selector);
        }

        IBodyClause IBodyClause.Clone(CloneContext cloneContext)
        {
            return Clone(cloneContext);
        }

        public string TranslateIntoName()
        {
            if (FuncIntoName != null)
                return FuncIntoName(intoIdentifier);
            return IntoName;
        }

        public GroupByClause Clone(CloneContext cloneContext)
        {
            LinqUtility.CheckNotNull("cloneContext", cloneContext);

            var clone = new GroupByClause(Selector, lambdaSelector, intoIdentifier);
            return clone;
        }

        public override string ToString()
        {
            return "collect " + _selector;
        }
    }
}