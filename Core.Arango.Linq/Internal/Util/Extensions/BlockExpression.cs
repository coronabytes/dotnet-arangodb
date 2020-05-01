using System.Linq;
using System.Linq.Expressions;

namespace Core.Arango.Linq.Internal.Util.Extensions
{
    internal static class BlockExpressionExtensions
    {
        public static bool HasMultipleLines(this BlockExpression expr)
        {
            return expr.Variables.Any() || expr.Expressions.Count > 1;
        }

        public static bool HasVariablesRecursive(this BlockExpression expr)
        {
            return expr.Variables.Any() ||
                   expr.Expressions.OfType<BlockExpression>().Any(x => x.HasVariablesRecursive());
        }
    }
}