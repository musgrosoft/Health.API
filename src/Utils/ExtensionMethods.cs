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

        public static DateTime GetWeekStartingOnMonday(this DateTime val)
        {
            //set Monday as first day of week
            int day = (DateTime.Now.DayOfWeek == 0) 
                ? 7 
                : (int)DateTime.Now.DayOfWeek;

            day = day - 1;

            return val.AddDays(-day);
        }

        public static DateTime GetFirstDayOfMonth(this DateTime val)
        {
            return new DateTime(val.Year, val.Month, 1);
        }
    }
}
