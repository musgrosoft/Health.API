using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

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

        public static double ToUnixTimeMillisecondsFromDate(this DateTime dateTime)
        {
            return (dateTime - epoch).TotalMilliseconds;
        }

        public static bool Between(this DateTime val, DateTime startDate, DateTime endDate)
        {
            return (val >= startDate && val <= endDate);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;  
        }

        public static T FromJSONTo<T>(this String content) where T : new()
        {
           return JsonConvert.DeserializeObject<T>(content);
        }

        public static IEnumerable<T> FromCSVToIEnumerableOf<T>(this String csv) where T : new()
        {
            var listT = new List<T>();

            var lines = csv.Replace("\"","").Split("\n");

            var propertyNames = lines.First().Split(',');


            Type myType = typeof(T);

            var propertyInfos = new Dictionary<string, PropertyInfo>();


            foreach (var propertyName in propertyNames)
            {
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                if (myPropInfo != null)
                {
                    propertyInfos.Add(propertyName, myPropInfo);
                }
            }



            foreach (var line in lines.Skip(1))
            {
                try
                {
                    var values = line.Split(',');
                    var elementT = new T();

                    for (int i = 0; i < propertyNames.Length; i++)
                    {
                        if (propertyInfos.ContainsKey(propertyNames[i]))
                        {
                            var value = values[i];

                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                var propInfo = propertyInfos[propertyNames[i]];

                                var typedValue = Convert.ChangeType(value, propInfo.PropertyType);

                                propInfo.SetValue(elementT, typedValue);
                            }
                        }
                    }

                    listT.Add(elementT);
                }
                catch (Exception ex)
                {

                }

            }


            return listT;
        }
//
//        public static T Convert<T>(this string input)
//        {
//            var converter = TypeDescriptor.GetConverter(typeof(T));
//            if (converter != null)
//            {
//                //Cast ConvertFromString(string text) : object to (T)
//                return (T)converter.ConvertFromString(input);
//            }
//            return default(T);
//        }
    }
}
