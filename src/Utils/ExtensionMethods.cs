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

        //public static DateTime ToDateFromUnixTimeMilliseconds(this long val)
        //{
        //    return epoch.AddMilliseconds(val);
        //}

        //public static DateTime ToDateFromUnixTimeMilliseconds(this int val)
        //{
        //    return epoch.AddMilliseconds(val);
        //}

        public static double ToUnixTimeFromDate(this DateTime dateTime)
        {
            return (dateTime - epoch).TotalSeconds;
        }

        // public static double ToUnixTimeMillisecondsFromDate(this DateTime dateTime)
        // {
        //     return (dateTime - epoch).TotalMilliseconds;
        // }

        public static bool Between(this DateTime val, DateTime startDate, DateTime endDate)
        {
            return (val >= startDate && val <= endDate);
        }

        // public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        // {
        //     int diff = dt.DayOfWeek - startOfWeek;
        //     if (diff < 0)
        //     {
        //         diff += 7;
        //     }
        //     return dt.AddDays(-1 * diff).Date;  
        // }


        
    }
}
