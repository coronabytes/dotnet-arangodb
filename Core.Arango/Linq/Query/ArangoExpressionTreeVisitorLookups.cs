using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Arango.Linq.Query
{
    internal partial class ArangoExpressionTreeVisitor
    {
        private static readonly Dictionary<ExpressionType, string> expressionTypes;
        //private static readonly Dictionary<string, string> aqlMethods;

        private static readonly HashSet<string> methodsWithFirstGenericArgument;
        private static readonly HashSet<string> methodsWithSecondGenericArgument;
        private static readonly Dictionary<string, string> methodsWithNoParenthesis;

        static ArangoExpressionTreeVisitor()
        {
            methodsWithFirstGenericArgument = new HashSet<string>
            {
                "near", "within", "within_rectangle", "edges", "neighbors", "traversal", "traversal_tree",
                "shortest_path", "paths"
            };

            methodsWithSecondGenericArgument = new HashSet<string>
            {
                "neighbors", "traversal", "traversal_tree", "shortest_path", "paths"
            };

            methodsWithNoParenthesis = new Dictionary<string, string>
            {
                ["in"] = "in"
            };

            expressionTypes = new Dictionary<ExpressionType, string>
            {
                {ExpressionType.Equal, " == "},
                {ExpressionType.NotEqual, " != "},
                {ExpressionType.LessThan, " < "},
                {ExpressionType.LessThanOrEqual, " <= "},
                {ExpressionType.GreaterThan, " > "},
                {ExpressionType.GreaterThanOrEqual, " >= "},
                {ExpressionType.And, " and "},
                {ExpressionType.AndAlso, " and "},
                {ExpressionType.Or, " or "},
                {ExpressionType.OrElse, " or "},
                {ExpressionType.Not, " not "},
                {ExpressionType.Add, " + "},
                {ExpressionType.Subtract, " - "},
                {ExpressionType.Multiply, " * "},
                {ExpressionType.Divide, " / "},
                {ExpressionType.Modulo, " % "}
            };
        }
    }
}