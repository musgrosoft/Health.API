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

        public static DateTime HealthWeek(this DateTime val)
        {
            return val.AddDays(-(int)val.DayOfWeek);
        }

        public static DateTime HealthMonth(this DateTime val)
        {
            return new DateTime(val.Year, val.Month, 1);
        }
    }
}
