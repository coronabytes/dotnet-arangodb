using System.Diagnostics.CodeAnalysis;
using Core.Arango.Linq.Attributes;

namespace Core.Arango.Linq
{
    [SuppressMessage("CodeQuality", "IDE0060")]
    public partial class Aql
    {
        /// <summary>
        ///     Concatenate the values passed as value1 to valueN.
        /// </summary>
        [AqlFunction("CONCAT")]
        public static string Concat(params string[] str)
        {
            throw E;
        }

        /// <summary>
        ///     Concatenate the strings passed as arguments value1 to valueN using the separator string.
        /// </summary>
        [AqlFunction("CONCAT_SEPARATOR")]
        public static string ConcatSeparator(string separator, params string[] str)
        {
            throw E;
        }

        /// <summary>
        ///     Return the number of characters in value (not byte length).
        /// </summary>
        [AqlFunction("CHAR_LENGTH")]
        public static int CharLength(string value)
        {
            throw E;
        }

        /// <summary>
        ///     Convert upper-case letters in value to their lower-case counterparts. All other characters are returned unchanged.
        /// </summary>
        [AqlFunction("LOWER")]
        public static string Lower(string value)
        {
            throw E;
        }

        /// <summary>
        ///     Convert lower-case letters in value to their upper-case counterparts. All other characters are returned unchanged.
        /// </summary>
        [AqlFunction("UPPER")]
        public static string Upper(string value)
        {
            throw E;
        }

        /// <summary>
        ///     Replace search values in the string value.
        /// </summary>
        [AqlFunction("SUBSTITUTE")]
        public static string Substitute(string value, string search, string replace)
        {
            throw E;
        }

        /// <summary>
        ///     Replace search values in the string value.
        /// </summary>
        [AqlFunction("SUBSTITUTE")]
        public static string Substitute(string value, string search, string replace, int limit)
        {
            throw E;
        }

        /// <summary>
        ///     Return a substring of value.
        /// </summary>
        [AqlFunction("SUBSTRING")]
        public static string Substring(string value, int offset)
        {
            throw E;
        }

        /// <summary>
        ///     Return a substring of value.
        /// </summary>
        [AqlFunction("SUBSTRING")]
        public static string Substring(string value, int offset, int length)
        {
            throw E;
        }

        /// <summary>
        ///     Return the n leftmost characters of the string value.
        /// </summary>
        [AqlFunction("LEFT")]
        public static string Left(string value, int length)
        {
            throw E;
        }

        /// <summary>
        ///     Return the length rightmost characters of the string value.
        /// </summary>
        [AqlFunction("RIGHT")]
        public static string Right(string value, int length)
        {
            throw E;
        }

        /// <summary>
        ///     Return the string value with whitespace stripped from the start and/or end.
        /// </summary>
        [AqlFunction("TRIM")]
        public static string Trim(string value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the string value with whitespace stripped from the start and/or end.
        /// </summary>
        [AqlFunction("TRIM")]
        public static string Trim(string value, int type)
        {
            throw E;
        }

        /// <summary>
        ///     Return the string value with whitespace stripped from the start and/or end.
        /// </summary>
        [AqlFunction("TRIM")]
        public static string Trim(string value, string chars)
        {
            throw E;
        }

        /// <summary>
        ///     Return the string value with whitespace stripped from the start only.
        /// </summary>
        [AqlFunction("LTRIM")]
        public static string LTrim(string value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the string value with whitespace stripped from the start only.
        /// </summary>
        [AqlFunction("LTRIM")]
        public static string LTrim(string value, string chars)
        {
            throw E;
        }

        /// <summary>
        ///     RTRIM(value, chars) → strippedString
        /// </summary>
        [AqlFunction("RTRIM")]
        public static string RTrim(string value)
        {
            throw E;
        }

        /// <summary>
        ///     RTRIM(value, chars) → strippedString
        /// </summary>
        [AqlFunction("RTRIM")]
        public static string RTrim(string value, string chars)
        {
            throw E;
        }

        /// <summary>
        ///     Split the given string value into a list of strings, using the separator.
        /// </summary>
        [AqlFunction("SPLIT")]
        public static string[] Split(string value)
        {
            throw E;
        }

        /// <summary>
        ///     Split the given string value into a list of strings, using the separator.
        /// </summary>
        [AqlFunction("SPLIT")]
        public static string[] Split(string value, string separator)
        {
            throw E;
        }

        /// <summary>
        ///     Split the given string value into a list of strings, using the separator.
        /// </summary>
        [AqlFunction("SPLIT")]
        public static string[] Split(string value, string separator, int limit)
        {
            throw E;
        }

        /// <summary>
        ///     Return the reverse of the string value.
        /// </summary>
        [AqlFunction("REVERSE")]
        public static string Reverse(string value)
        {
            throw E;
        }

        /// <summary>
        ///     Check whether the string search is contained in the string text. The string matching performed by CONTAINS is
        ///     case-sensitive.
        /// </summary>
        [AqlFunction("CONTAINS")]
        public static bool Contains(string text, string search)
        {
            throw E;
        }

        /// <summary>
        ///     Check whether the string search is contained in the string text. The string matching performed by CONTAINS is
        ///     case-sensitive.
        /// </summary>
        [AqlFunction("CONTAINS")]
        public static int Contains(string text, string search, bool returnIndex)
        {
            throw E;
        }

        /// <summary>
        ///     Return the position of the first occurrence of the string search inside the string text. Positions start at 0.
        /// </summary>
        [AqlFunction("FIND_FIRST")]
        public static int FindFirst(string text, string search)
        {
            throw E;
        }

        /// <summary>
        ///     Return the position of the first occurrence of the string search inside the string text. Positions start at 0.
        /// </summary>
        [AqlFunction("FIND_FIRST")]
        public static int FindFirst(string text, string search, int start, int end)
        {
            throw E;
        }

        /// <summary>
        ///     Return the position of the last occurrence of the string search inside the string text. Positions start at 0.
        /// </summary>
        [AqlFunction("FIND_LAST")]
        public static int FindLast(string text, string search)
        {
            throw E;
        }

        /// <summary>
        ///     Return the position of the last occurrence of the string search inside the string text. Positions start at 0.
        /// </summary>
        [AqlFunction("FIND_LAST")]
        public static int FindLast(string text, string search, int start, int end)
        {
            throw E;
        }

        /// <summary>
        ///     Check whether the pattern search is contained in the string text, using wildcard matching.
        /// </summary>
        [AqlFunction("LIKE")]
        public static bool Like(string text, string search)
        {
            throw E;
        }

        /// <summary>
        ///     Check whether the pattern search is contained in the string text, using wildcard matching.
        /// </summary>
        [AqlFunction("LIKE")]
        public static bool Like(string text, string search, bool caseInsensitive)
        {
            throw E;
        }
    }
}