using System.Diagnostics.CodeAnalysis;
using Core.Arango.Linq.Attributes;

namespace Core.Arango.Linq
{
    [SuppressMessage("CodeQuality", "IDE0060")]
    public partial class Aql
    {
        [AqlFunction("CONCAT")]
        public static string Concat(params string[] str)
        {
            throw E;
        }

        [AqlFunction("CONCAT_SEPARATOR")]
        public static string ConcatSeparator(string separator, params string[] str)
        {
            throw E;
        }

        [AqlFunction("CHAR_LENGTH")]
        public static int CharLength(string value)
        {
            throw E;
        }

        [AqlFunction("LOWER")]
        public static string Lower(string value)
        {
            throw E;
        }

        [AqlFunction("UPPER")]
        public static string Upper(string value)
        {
            throw E;
        }

        [AqlFunction("SUBSTITUTE")]
        public static string Substitute(string value, string search, string replace)
        {
            throw E;
        }

        [AqlFunction("SUBSTITUTE")]
        public static string Substitute(string value, string search, string replace, int limit)
        {
            throw E;
        }

        [AqlFunction("SUBSTRING")]
        public static string Substring(string value, int offset)
        {
            throw E;
        }

        [AqlFunction("SUBSTRING")]
        public static string Substring(string value, int offset, int length)
        {
            throw E;
        }

        [AqlFunction("LEFT")]
        public static string Left(string value, int length)
        {
            throw E;
        }

        [AqlFunction("RIGHT")]
        public static string Right(string value, int length)
        {
            throw E;
        }

        [AqlFunction("TRIM")]
        public static string Trim(string value)
        {
            throw E;
        }

        [AqlFunction("TRIM")]
        public static string Trim(string value, int type)
        {
            throw E;
        }

        [AqlFunction("TRIM")]
        public static string Trim(string value, string chars)
        {
            throw E;
        }

        [AqlFunction("LTRIM")]
        public static string LTrim(string value)
        {
            throw E;
        }

        [AqlFunction("LTRIM")]
        public static string LTrim(string value, string chars)
        {
            throw E;
        }

        [AqlFunction("RTRIM")]
        public static string RTrim(string value)
        {
            throw E;
        }

        [AqlFunction("RTRIM")]
        public static string RTrim(string value, string chars)
        {
            throw E;
        }

        [AqlFunction("SPLIT")]
        public static string[] Split(string value)
        {
            throw E;
        }

        [AqlFunction("SPLIT")]
        public static string[] Split(string value, string separator)
        {
            throw E;
        }

        [AqlFunction("SPLIT")]
        public static string[] Split(string value, string separator, int limit)
        {
            throw E;
        }

        [AqlFunction("REVERSE")]
        public static string Reverse(string value)
        {
            throw E;
        }

        [AqlFunction("CONTAINS")]
        public static bool Contains(string text, string search)
        {
            throw E;
        }

        [AqlFunction("CONTAINS")]
        public static int Contains(string text, string search, bool returnIndex)
        {
            throw E;
        }

        [AqlFunction("FIND_FIRST")]
        public static int FindFirst(string text, string search)
        {
            throw E;
        }

        [AqlFunction("FIND_FIRST")]
        public static int FindFirst(string text, string search, int start, int end)
        {
            throw E;
        }

        [AqlFunction("FIND_LAST")]
        public static int FindLast(string text, string search)
        {
            throw E;
        }

        [AqlFunction("FIND_LAST")]
        public static int FindLast(string text, string search, int start, int end)
        {
            throw E;
        }

        [AqlFunction("LIKE")]
        public static bool Like(string text, string search)
        {
            throw E;
        }

        [AqlFunction("LIKE")]
        public static bool Like(string text, string search, bool caseInsensitive)
        {
            throw E;
        }
    }
}