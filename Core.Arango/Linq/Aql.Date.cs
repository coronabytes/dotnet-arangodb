using System;
using System.Diagnostics.CodeAnalysis;
using Core.Arango.Linq.Attributes;

namespace Core.Arango.Linq
{
    [SuppressMessage("CodeQuality", "IDE0060")]
    public partial class Aql
    {
        /// <summary>
        ///     Get the current unix time as numeric timestamp.
        /// </summary>
        [AqlFunction("DATE_NOW")]
        public static long DateNow()
        {
            throw E;
        }

        /// <summary>
        ///     Return an ISO 8601 date time string from date.
        /// </summary>
        [AqlFunction("DATE_ISO8601")]
        public static DateTime DateIso8601(long timestamp)
        {
            throw E;
        }

        /// <summary>
        ///     Create a timestamp value from date.
        /// </summary>
        [AqlFunction("DATE_TIMESTAMP")]
        public static long DateTimestamp(DateTime timestamp)
        {
            throw E;
        }

        /// <summary>
        ///     Return the weekday number of date.
        /// </summary>
        [AqlFunction("DATE_DAYOFWEEK")]
        public static int DateDayOfWeek(DateTime date)
        {
            throw E;
        }

        /// <summary>
        ///     Return the year of date.
        /// </summary>
        [AqlFunction("DATE_YEAR")]
        public static int DateYear(DateTime date)
        {
            throw E;
        }

        /// <summary>
        ///     Return the month of date.
        /// </summary>
        [AqlFunction("DATE_MONTH")]
        public static int DateMonth(DateTime date)
        {
            throw E;
        }

        /// <summary>
        ///     Return the day of date.
        /// </summary>
        [AqlFunction("DATE_DAY")]
        public static int DateDay(DateTime date)
        {
            throw E;
        }

        /// <summary>
        ///     Return the hour of date.
        /// </summary>
        [AqlFunction("DATE_HOUR")]
        public static int DateHour(DateTime date)
        {
            throw E;
        }

        /// <summary>
        ///     Return the minute of date.
        /// </summary>
        [AqlFunction("DATE_MINUTE")]
        public static int DateMinute(DateTime date)
        {
            throw E;
        }

        /// <summary>
        ///     Return the second of date.
        /// </summary>
        [AqlFunction("DATE_SECOND")]
        public static int DateSecond(DateTime date)
        {
            throw E;
        }

        /// <summary>
        ///     Return the millisecond of date.
        /// </summary>
        [AqlFunction("DATE_MILLISECOND")]
        public static int DateMillisecond(DateTime date)
        {
            throw E;
        }

        /// <summary>
        ///     Return the day of year of date.
        /// </summary>
        [AqlFunction("DateDayOfYear")]
        public static int DATE_DAYOFYEAR(DateTime date)
        {
            throw E;
        }

        /// <summary>
        ///     Return the week date of date according to ISO 8601.
        /// </summary>
        [AqlFunction("DATE_ISOWEEK")]
        public static int DateIsoWeek(DateTime date)
        {
            throw E;
        }

        /// <summary>
        ///     Return whether date is in a leap year.
        /// </summary>
        [AqlFunction("DATE_LEAPYEAR")]
        public static bool DateLeapYear(DateTime date)
        {
            throw E;
        }

        /// <summary>
        ///     Return which quarter date belongs to.
        /// </summary>
        [AqlFunction("DATE_QUARTER")]
        public static int DateQuarter(DateTime date)
        {
            throw E;
        }

        /// <summary>
        ///     Return the number of days in the month of date.
        /// </summary>
        [AqlFunction("DATE_DAYS_IN_MONTH")]
        public static int DateDaysInMonth(DateTime date)
        {
            throw E;
        }

        /// <summary>
        ///     Truncates the given date after unit and returns the modified date.
        /// </summary>
        [AqlFunction("DATE_TRUNC")]
        public static DateTime DateTrunc(DateTime date, string unit)
        {
            throw E;
        }

        /// <summary>
        ///     Bin a date/time into a set of equal-distance buckets, to be used for grouping.
        /// </summary>
        [AqlFunction("DATE_ROUND")]
        public static DateTime DateRound(DateTime date, int amount, string unit)
        {
            throw E;
        }

        /// <summary>
        ///     Format a date according to the given format string.
        /// </summary>
        [AqlFunction("DATE_FORMAT")]
        public static string DateFormat(DateTime date, string format)
        {
            throw E;
        }

        /// <summary>
        ///     Add amount given in unit to date and return the calculated date.
        /// </summary>
        [AqlFunction("DATE_ADD")]
        public static DateTime DateAdd(DateTime date, int amount, string unit)
        {
            throw E;
        }

        /// <summary>
        ///     Add amount given in unit to date and return the calculated date.
        /// </summary>
        [AqlFunction("DATE_ADD")]
        public static DateTime DateAdd(DateTime date, string amount)
        {
            throw E;
        }

        /// <summary>
        ///     Subtract amount given in unit from date and return the calculated date.
        /// </summary>
        [AqlFunction("DATE_SUBTRACT")]
        public static DateTime DateSubtract(DateTime date, int amount, string unit)
        {
            throw E;
        }

        /// <summary>
        ///     Subtract amount given in unit from date and return the calculated date.
        /// </summary>
        [AqlFunction("DATE_SUBTRACT")]
        public static DateTime DateSubtract(DateTime date, string amount)
        {
            throw E;
        }

        /// <summary>
        ///     Calculate the difference between two dates in given time unit, optionally with decimal places.
        /// </summary>
        [AqlFunction("DATE_DIFF")]
        public static long DateDiff(DateTime date1, DateTime date2)
        {
            throw E;
        }

        /// <summary>
        ///     Calculate the difference between two dates in given time unit, optionally with decimal places.
        /// </summary>
        [AqlFunction("DATE_DIFF")]
        public static double DateDiff(DateTime date1, DateTime date2, bool asFloat)
        {
            throw E;
        }

        /// <summary>
        ///     Check if two partial dates match.
        /// </summary>
        [AqlFunction("DATE_COMPARE")]
        public static bool DateCompare(DateTime date1, DateTime date2, string unitRangeStart)
        {
            throw E;
        }

        /// <summary>
        ///     Check if two partial dates match.
        /// </summary>
        [AqlFunction("DATE_COMPARE")]
        public static bool DateCompare(DateTime date1, DateTime date2, string unitRangeStart, string unitRangeEnd)
        {
            throw E;
        }

        /// <summary>
        ///     Converts date assumed in Zulu time (UTC) to local timezone.
        /// </summary>
        [AqlFunction("DATE_UTCTOLOCAL")]
        public static DateTime DateUtcToLocal(DateTime date, string timezone)
        {
            throw E;
        }

        /// <summary>
        ///     Converts date assumed in local timezone to Zulu time (UTC).
        /// </summary>
        [AqlFunction("DATE_LOCALTOUTC")]
        public static DateTime DateLocalToUtc(DateTime date, string timezone)
        {
            throw E;
        }

        /// <summary>
        ///     Returns system timezone ArangoDB is running on.
        /// </summary>
        [AqlFunction("DATE_TIMEZONE")]
        public static string DateTimeZone()
        {
            throw E;
        }

        /// <summary>
        ///     Returns all valid timezone names.
        /// </summary>
        [AqlFunction("DATE_TIMEZONES")]
        public static string[] DateTimeZones()
        {
            throw E;
        }
    }
}