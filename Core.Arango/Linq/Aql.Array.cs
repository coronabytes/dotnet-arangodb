using System.Diagnostics.CodeAnalysis;
using Core.Arango.Linq.Attributes;

namespace Core.Arango.Linq
{
    [SuppressMessage("CodeQuality", "IDE0060")]
    public partial class Aql
    {
        /// <summary>
        ///     Add all elements of an array to another array. All values are added at the end of the array (right side).
        /// </summary>
        [AqlFunction("APPEND")]
        public static T[] Append<T>(T[] anyArray, T[] values)
        {
            throw E;
        }

        /// <summary>
        ///     Add all elements of an array to another array. All values are added at the end of the array (right side).
        /// </summary>
        [AqlFunction("APPEND")]
        public static T[] Append<T>(T[] anyArray, T[] values, bool unique)
        {
            throw E;
        }

        /// <summary>
        ///     Get the number of distinct elements in an array.
        /// </summary>
        [AqlFunction("COUNT_DISTINCT")]
        public static long CountDistinct<T>(T[] anyArray)
        {
            throw E;
        }

        /// <summary>
        ///     This is an alias for COUNT_DISTINCT().
        /// </summary>
        [AqlFunction("COUNT_UNIQUE")]
        public static long CountUnique<T>(T[] anyArray)
        {
            throw E;
        }

        /// <summary>
        ///     Get the first element of an array. It is the same as anyArray[0].
        /// </summary>
        [AqlFunction("FIRST")]
        public static T First<T>(T[] anyArray)
        {
            throw E;
        }


        /// <summary>
        ///     Turn an array of arrays into a flat array.
        /// </summary>
        [AqlFunction("FLATTEN")]
        public static T[] Flatten<T>(object anyArray)
        {
            throw E;
        }

        /// <summary>
        ///     Turn an array of arrays into a flat array.
        /// </summary>
        [AqlFunction("FLATTEN")]
        public static T[] Flatten<T>(object anyArray, int depth)
        {
            throw E;
        }

        /// <summary>
        ///     Accepts an arbitrary number of arrays and produces a new array with the elements interleaved.
        /// </summary>
        [AqlFunction("INTERLEAVE")]
        public static T[] Interleave<T>(params T[] arrays)
        {
            throw E;
        }

        /// <summary>
        ///     Return the intersection of all arrays specified.
        /// </summary>
        [AqlFunction("INTERSECTION")]
        public static T[] Intersection<T>(params T[] arrays)
        {
            throw E;
        }
    }
}