using System;
using System.Diagnostics.CodeAnalysis;
using Core.Arango.Linq.Attributes;

namespace Core.Arango.Linq
{
    [SuppressMessage("CodeQuality", "IDE0060")]
    public partial class Aql
    {
        /// <summary>
        ///     Return the first element that is not null, and null if all alternatives are null themselves. It is also known as
        ///     COALESCE() in SQL.
        /// </summary>
        [AqlFunction("NOT_NULL")]
        public static T NotNull<T>(params object[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the first alternative that is an array, and null if none of the alternatives is an array.
        /// </summary>
        [AqlFunction("FIRST_LIST")]
        public static T[] FirstList<T>(params object[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the first alternative that is a document, and null if none of the alternatives is a document.
        /// </summary>
        [AqlFunction("FIRST_DOCUMENT")]
        public static T FirstDocument<T>(params object[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Return an array of collections.
        /// </summary>
        [AqlFunction("COLLECTIONS")]
        public static T[] Collections<T>()
        {
            throw E;
        }

        /// <summary>
        ///     Return the name of the current user.
        /// </summary>
        [AqlFunction("CURRENT_USER")]
        public static string CurrentUser()
        {
            throw E;
        }

        /// <summary>
        ///     Decompose the specified revision string into its components.
        /// </summary>
        [AqlFunction("DECODE_REV")]
        public static (DateTime date, long count)? DecodeRev(string revision)
        {
            throw E;
        }

        /// <summary>
        ///     Return the document which is uniquely identified by its id.
        /// </summary>
        [AqlFunction("DOCUMENT")]
        public static T Document<T>(string collection, object key)
        {
            throw E;
        }

        /// <summary>
        ///     Return the document which is uniquely identified by its id.
        /// </summary>
        [AqlFunction("DOCUMENT")]
        public static T Document<T>(string id)
        {
            throw E;
        }

        /// <summary>
        ///     Return the document which is uniquely identified by its id.
        /// </summary>
        [AqlFunction("DOCUMENT")]
        public static T[] Document<T>(string collection, object[] keys)
        {
            throw E;
        }

        /// <summary>
        ///     Return the document which is uniquely identified by its id.
        /// </summary>
        [AqlFunction("DOCUMENT")]
        public static T[] Document<T>(string[] ids)
        {
            throw E;
        }

        /// <summary>
        ///     Determine the amount of documents in a collection. LENGTH() can also determine the number of elements in an array,
        ///     the number of attribute keys of an object / document and the character length of a string.
        /// </summary>
        [AqlFunction("LENGTH")]
        public static long Length(object collection)
        {
            throw E;
        }

        /// <summary>
        ///     Calculate a hash value for value.
        /// </summary>
        [AqlFunction("HASH")]
        public static long Hash(object value)
        {
            throw E;
        }

        /// <summary>
        ///     Dynamically call the function funcName with the arguments specified. Arguments are given as array and are passed as
        ///     separate parameters to the called function.
        /// </summary>
        [AqlFunction("APPLY")]
        public static T Apply<T>(string funcName, object[] arguments)
        {
            throw E;
        }

        /// <summary>
        ///     Dynamically call the function funcName with the arguments specified. Arguments are given as multiple parameters and
        ///     passed as separate parameters to the called function.
        /// </summary>
        [AqlFunction("CALL")]
        public static T Call<T>(string funcName, params object[] arguments)
        {
            throw E;
        }

        /// <summary>
        ///     if the expression evaluates to false ASSERT will throw an error.
        /// </summary>
        [AqlFunction("ASSERT")]
        public static bool Assert(bool vs, string message)
        {
            throw E;
        }

        /// <summary>
        ///     If the expression evaluates to false WARN will issue a warning.
        /// </summary>
        [AqlFunction("WARN")]
        public static bool Warn(bool v, string message)
        {
            throw E;
        }

        /// <summary>
        ///     Returns true if value is greater than (or equal to) low and less than (or equal to) high. The values can be of
        ///     different types. They are compared as described in Type and value order and is thus identical to the comparison
        ///     operators &lt;, &lt;=, &gt; and &gt;= in behavior.
        /// </summary>
        [AqlFunction("IN_RANGE")]
        public static bool InRange<T>(T value, T low, T high, bool includeLow, bool includeHigh)
        {
            throw E;
        }

        /// <summary>
        ///     Allows to access results of a Pregel job that are only held in memory.
        /// </summary>
        [AqlFunction("PREGEL_RESULT")]
        public static T[] PregelResult<T>(string id, bool withId)
        {
            throw E;
        }
    }
}