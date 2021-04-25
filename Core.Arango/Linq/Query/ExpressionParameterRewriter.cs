using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Arango.Linq.Query
{
    internal class ExpressionParameterRewriter
    {
        public static Expression<Func<T1, T2, T3>> RewriteParameters<T1, T2, T3>(Expression<Func<T1, T2, T3>> exp,
            params string[] newNames)
        {
            if (newNames.Length != 2)
                throw new InvalidOperationException("Invalid number of parameteres new names");

            var newExpression = exp;
            for (var i = 0; i < newNames.Length; i++)
                newExpression = RewriteParameterAt(newExpression, i, newNames[i]);

            return newExpression;
        }

        public static Expression<Func<T1, T2, T3>> RewriteParameterAt<T1, T2, T3>(Expression<Func<T1, T2, T3>> exp,
            int index, string newName)
        {
            var param = Expression.Parameter(exp.Parameters[index].Type, newName);
            var unchangedParams = exp.Parameters.ToList();
            unchangedParams.RemoveAt(index);

            var newExpression = new ExpressionParameterRewriterVisitor(param, unchangedParams).Visit(exp);
            return (Expression<Func<T1, T2, T3>>) newExpression;
        }

        private class ExpressionParameterRewriterVisitor : ExpressionVisitor
        {
            private readonly ParameterExpression _newParameterExpression;
            private readonly IList<ParameterExpression> _unchangedParametersExpressions;

            public ExpressionParameterRewriterVisitor(ParameterExpression parameterExpression,
                IList<ParameterExpression> unchangedParametersExpressions)
            {
                _newParameterExpression = parameterExpression;
                _unchangedParametersExpressions = unchangedParametersExpressions;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (_unchangedParametersExpressions.Count(p => p.Name == node.Name) == 0)
                    return _newParameterExpression;
                return node;
            }
        }
    }
}