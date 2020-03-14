using System;
using System.Reflection;

namespace Core.Arango.Linq.Internal.Util.Extensions
{
    internal static class MemberInfoExtensions
    {
        public static bool HasAttribute<TAttribute>(this MemberInfo mi, bool inherit = false)
            where TAttribute : Attribute
        {
            return mi.GetCustomAttributes(typeof(TAttribute), inherit).Any();
        }
    }
}