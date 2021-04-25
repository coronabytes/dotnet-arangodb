using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Core.Arango.Linq.Attributes;

namespace Core.Arango.Linq
{
    [SuppressMessage("CodeQuality", "IDE0060")]
    public partial class Aql
    {
        /// <summary>
        ///     Return the absolute part of value.
        /// </summary>
        [AqlFunction("ABS")]
        public static double Abs(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the arccosine of value.
        /// </summary>
        [AqlFunction("ACOS")]
        public static double Acos(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the arcsine of value.
        /// </summary>
        [AqlFunction("ASIN")]
        public static double Asin(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the arctangent of value.
        /// </summary>
        [AqlFunction("ATAN")]
        public static double Atan(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the arctangent of the quotient of y and x.
        /// </summary>
        [AqlFunction("ATAN2")]
        public static double Atan2(double y, double x)
        {
            throw E;
        }

        /// <summary>
        ///     Return the average (arithmetic mean) of the values in array.
        /// </summary>
        [AqlFunction("AVERAGE")]
        public static double Average(double[] value)
        {
            throw E;
        }

        /// <summary>
        ///     This is an alias for AVERAGE().
        /// </summary>
        [AqlFunction("AVG")]
        public static double Avg(double[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the integer closest but not less than value.
        /// </summary>
        [AqlFunction("CEIL")]
        public static double Ceil(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the cosine of value.
        /// </summary>
        [AqlFunction("COS")]
        public static double Cos(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the angle converted from radians to degrees.
        /// </summary>
        [AqlFunction("DEGREES")]
        public static double Degrees(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return Euler’s constant raised to the power of value.
        /// </summary>
        [AqlFunction("EXP")]
        public static double Exp(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return 2 raised to the power of value.
        /// </summary>
        [AqlFunction("EXP2")]
        public static double Exp2(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the integer closest but not greater than value.
        /// </summary>
        [AqlFunction("FLOOR")]
        public static double Floor(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the natural logarithm of value
        /// </summary>
        [AqlFunction("LOG")]
        public static double Log(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the base 2 logarithm of value.
        /// </summary>
        [AqlFunction("LOG2")]
        public static double Log2(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the base 10 logarithm of value.
        /// </summary>
        [AqlFunction("LOG10")]
        public static double Log10(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the greatest element of anyArray. The array is not limited to numbers. Also see type and value order.
        /// </summary>
        [AqlFunction("MAX")]
        public static T Max<T>(T[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the median value of the values in array.
        /// </summary>
        [AqlFunction("MEDIAN")]
        public static double Median(double[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the smallest element of anyArray. The array is not limited to numbers.
        /// </summary>
        [AqlFunction("MIN")]
        public static T Min<T>(T[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the nth percentile of the values in numArray.
        /// </summary>
        [AqlFunction("PERCENTILE")]
        public static double? Percentile(double[] value, double n, string method)
        {
            throw E;
        }

        /// <summary>
        ///     Return pi.
        /// </summary>
        [AqlFunction("PI")]
        public static double Pi()
        {
            throw E;
        }

        /// <summary>
        ///     Return the base to the exponent exp.
        /// </summary>
        [AqlFunction("POW")]
        public static double Round(double @base, double exp)
        {
            throw E;
        }

        /// <summary>
        ///     Return the product of the values in array.
        /// </summary>
        [AqlFunction("PRODUCT")]
        public static double Product(double[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the angle converted from degrees to radians.
        /// </summary>
        [AqlFunction("RADIANS")]
        public static double Radians(double[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Return a pseudo-random number between 0 and 1.
        /// </summary>
        [AqlFunction("RAND")]
        public static double Rand()
        {
            throw E;
        }

        /// <summary>
        ///     Return an array of numbers in the specified range, optionally with increments other than 1. The start and stop
        ///     arguments are truncated to integers unless a step argument is provided.
        /// </summary>
        [AqlFunction("Range")]
        public static IEnumerable<double> Range(double start, double end, double step)
        {
            throw E;
        }

        /// <summary>
        ///     Return an array of numbers in the specified range, optionally with increments other than 1. The start and stop
        ///     arguments are truncated to integers unless a step argument is provided.
        /// </summary>
        [AqlFunction("Range")]
        public static IEnumerable<long> Range(long start, long end)
        {
            throw E;
        }

        /// <summary>
        ///     Return the integer closest to value.
        /// </summary>
        [AqlFunction("ROUND")]
        public static long Round(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the sine of value.
        /// </summary>
        [AqlFunction("SIN")]
        public static double Sin(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the square root of value.
        /// </summary>
        [AqlFunction("SQRT")]
        public static double Sqrt(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the population standard deviation of the values in array.
        /// </summary>
        [AqlFunction("STDDEV_POPULATION")]
        public static double StdDevPopulation(double[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the sample standard deviation of the values in array.
        /// </summary>
        [AqlFunction("STDDEV_SAMPLE")]
        public static double StdDevSample(double[] value)
        {
            throw E;
        }

        /// <summary>
        ///     This is an alias for STDDEV_POPULATION().
        /// </summary>
        [AqlFunction("STDDEV")]
        public static double StdDev(double[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the sum of the values in array.
        /// </summary>
        [AqlFunction("SUM")]
        public static double Sum(double[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the tangent of value.
        /// </summary>
        [AqlFunction("TAN")]
        public static double Tan(double value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the population variance of the values in array.
        /// </summary>
        [AqlFunction("VARIANCE_POPULATION")]
        public static double VariancePopulation(double[] value)
        {
            throw E;
        }

        /// <summary>
        ///     Return the sample variance of the values in array.
        /// </summary>
        [AqlFunction("VARIANCE_SAMPLE")]
        public static double VarianceSample(double[] value)
        {
            throw E;
        }

        /// <summary>
        ///     This is an alias for VARIANCE_POPULATION().
        /// </summary>
        [AqlFunction("VARIANCE")]
        public static double Variance(double[] value)
        {
            throw E;
        }
    }
}