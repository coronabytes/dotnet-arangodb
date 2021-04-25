using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core.Arango.Linq.Attributes;

namespace Core.Arango.Linq
{
    [SuppressMessage("CodeQuality", "IDE0060")]
    public partial class Aql
    {
        [AqlFunction("DATE_NOW")]
        public static long DateNow()
        {
            throw E;
        }

        [AqlFunction("DATE_ISO8601")]
        public static DateTime DateIso8601(long timestamp)
        {
            throw E;
        }

        [AqlFunction("DATE_TIMESTAMP")]
        public static long DateTimestamp(DateTime timestamp)
        {
            throw E;
        }
        
        [AqlFunction("DATE_ADD")]
        public static DateTime DateAdd(DateTime date, double amount, string unit)
        {
            throw E;
        }
    }
}