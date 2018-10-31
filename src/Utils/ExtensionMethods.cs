using System;

namespace Utils
{
    public static class ExtensionMethods
    {
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime ToDateFromUnixTime(this long val)
        {
            return epoch.AddSeconds(val);
        }

        public static DateTime ToDateFromUnixTime(this int val)
        {
            return epoch.AddSeconds(val);
        }

        public static double ToUnixTimeFromDate(this DateTime dateTime)
        {
            return (dateTime - epoch).TotalSeconds;
        }


        //public static DateTime GetWeekStartingOnMonday(this DateTime val)
        //{
        //    //set Monday as first day of week
        //    int day = (val.DayOfWeek == 0) 
        //        ? 7 
        //        : (int)val.DayOfWeek;

        //    day = day - 1;

        //    return val.AddDays(-day);
        //}

        //public static DateTime GetFirstDayOfMonth(this DateTime val)
        //{
        //    return new DateTime(val.Year, val.Month, 1);
        //}

        public static bool Between(this DateTime val, DateTime startDate, DateTime endDate)
        {
            return (val >= startDate && val <= endDate);
        }
    }
}
