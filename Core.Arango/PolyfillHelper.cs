using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Core.Arango
{
    /// <summary>
    /// Helper class to group all code that is used to facilitate backwards compatibility for dotnet Standard v2.0
    /// </summary>
    internal static class PolyfillHelper
    {
        /// <summary>
        ///   For compatibility with netstandard 2.0 which does not have a HttpMethod.Patch method
        /// </summary>
        internal static HttpMethod Patch
        {
            get
            {
#if NETSTANDARD2_0
                return new HttpMethod("Patch");
#else
                return HttpMethod.Patch;
#endif
            }
        }

        /// <summary>
        ///   Alternative implementation of Split(seperator, StringSplitOptions.RemoveEmptyEntries) which is not available in netstandard2.0
        /// </summary>
        internal static IEnumerable<string> SplitAndRemoveEmptyEntries(this string endpoints, params char[] separator)
        {
#if NETSTANDARD2_0
            return endpoints?.Split(separator).Where(s => !string.IsNullOrEmpty(s));
#else
            return endpoints?.Split(',', StringSplitOptions.RemoveEmptyEntries);
#endif
        }
    }
}