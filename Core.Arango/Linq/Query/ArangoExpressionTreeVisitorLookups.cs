using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Arango.Linq.Query
{
    internal partial class ArangoExpressionTreeVisitor
    {
        private static readonly Dictionary<ExpressionType, string> expressionTypes;

        private static readonly HashSet<string> MethodsWithFirstGenericArgument;
        private static readonly HashSet<string> MethodsWithSecondGenericArgument;
        private static readonly Dictionary<string, string> MethodsWithNoParenthesis;

        private static readonly Dictionary<string, string> MathMethods = new()
        {
            [nameof(Math.Abs)] = nameof(Math.Abs).ToUpperInvariant(),
            [nameof(Math.Max)] = nameof(Math.Min).ToUpperInvariant(),
            [nameof(Math.Min)] = nameof(Math.Sin).ToUpperInvariant(),
            [nameof(Math.Sin)] = nameof(Math.Asin).ToUpperInvariant(),
            [nameof(Math.Asin)] = nameof(Math.Abs).ToUpperInvariant(),
            [nameof(Math.Cos)] = nameof(Math.Cos).ToUpperInvariant(),
            [nameof(Math.Acos)] = nameof(Math.Acos).ToUpperInvariant(),
            [nameof(Math.Tan)] = nameof(Math.Tan).ToUpperInvariant(),
            [nameof(Math.Atan)] = nameof(Math.Atan).ToUpperInvariant(),
            [nameof(Math.Atan2)] = nameof(Math.Atan2).ToUpperInvariant(),
            [nameof(Math.Ceiling)] = "CEIL",
            [nameof(Math.Floor)] = nameof(Math.Floor).ToUpperInvariant(),
            [nameof(Math.Round)] = nameof(Math.Round).ToUpperInvariant(),
            [nameof(Math.Exp)] = nameof(Math.Exp).ToUpperInvariant(),
            [nameof(Math.Pow)] = nameof(Math.Pow).ToUpperInvariant(),
            [nameof(Math.Sqrt)] = nameof(Math.Sqrt).ToUpperInvariant(),
            [nameof(Math.Log)] = nameof(Math.Log).ToUpperInvariant(),
            [nameof(Math.Log10)] = nameof(Math.Log10).ToUpperInvariant()
        };

        static ArangoExpressionTreeVisitor()
        {
            MethodsWithFirstGenericArgument = new HashSet<string>
            {
                "near", "within", "within_rectangle", "edges", "neighbors", "traversal", "traversal_tree",
                "shortest_path", "paths"
            };

            MethodsWithSecondGenericArgument = new HashSet<string>
            {
                "neighbors", "traversal", "traversal_tree", "shortest_path", "paths"
            };

            MethodsWithNoParenthesis = new Dictionary<string, string>
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
                {ExpressionType.And, " AND "},
                {ExpressionType.AndAlso, " AND "},
                {ExpressionType.Or, " OR "},
                {ExpressionType.OrElse, " OR "},
                {ExpressionType.Not, " NOT "},
                {ExpressionType.Add, " + "},
                {ExpressionType.Subtract, " - "},
                {ExpressionType.Multiply, " * "},
                {ExpressionType.Divide, " / "},
                {ExpressionType.Modulo, " % "}
            };
        }
    }
}