using System;
using System.Collections.Generic;

namespace Core.Arango.Linq.Internal.Util
{
    internal static class Methods
    {
        public static void VerifyCount<T>(IList<T> lst, int count)
        {
            if (lst.Count == count) return;
            throw new InvalidOperationException("Invalid argument count");
        }

        public static void VerifyCount<T>(IList<T> lst, int? min, int? max)
        {
            if (min == null && max == null) throw new ArgumentNullException("Either 'min' or 'max' must be non-null");
            var valid = min == null || lst.Count >= min;
            valid = valid && (max == null || lst.Count <= max);
            if (valid) return;
            throw new InvalidOperationException("Invalid argument count");
        }
    }
}