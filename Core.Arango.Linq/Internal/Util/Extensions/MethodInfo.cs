using System.Linq;
using System.Reflection;

namespace Core.Arango.Linq.Internal.Util.Extensions
{
    internal static class MethodInfoExtensions
    {
        public static bool IsIndexerMethod(this MethodInfo mi)
        {
            return mi.In(
                mi.ReflectedType
                    .GetIndexers(true)
                    .SelectMany(x => new[] {x.GetMethod, x.SetMethod})
            );
        }

        public static bool IsIndexerMethod(this MethodInfo mi, out PropertyInfo? pi)
        {
            var indexerMethods = mi.ReflectedType
                .GetIndexers(true)
                .SelectMany(x => new[]
                {
                    (x, x.GetMethod),
                    (x, x.SetMethod)
                });
            foreach (var (x, method) in indexerMethods)
                if (method == mi)
                {
                    pi = x;
                    return true;
                }

            pi = null;
            return false;
        }
    }
}