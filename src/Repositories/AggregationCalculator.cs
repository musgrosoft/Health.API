using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories
{
    public class AggregationCalculator : IAggregationCalculator
    {
        private readonly HealthContext _healthContext;

        public AggregationCalculator(HealthContext healthContext)
        {
            _healthContext = healthContext;
        }

        public void AddMovingAverageTo<T>(
            IEnumerable<T> theList,
            Func<T, DateTime> dateTimeSelector,
            Func<T, Decimal> GetValue,
            Action<T, Decimal?> SetMovingAverage,
            int period = 10
        ) where T : class
        {
            var orderedList = theList.OrderBy(dateTimeSelector).ToList();

            for (int i = 0; i < orderedList.Count(); i++)
            {
                if (i >= period - 1)
                {
                    Decimal total = 0;
                    //to rewrite from i-period, ascending
                    for (int x = i; x > (i - period); x--)
                    {
                        total += GetValue(orderedList[x]);
                    }

                    decimal average = total / period;

                    SetMovingAverage(orderedList[i], average);
                }
                //else
                //{
                //    SetMovingAverage(orderedList[i], null);
                //}

            }

        }
        
        public void CalculateCumSumFor<T>(
            IEnumerable<T> theList,
            Func<T, DateTime> dateTimeSelector,
            Func<T, int?> GetValue,
            Func<T, int?> GetCumSum,
            Action<T, int?> SetCumSum
        ) where T : class
        {
            var orderedList = theList.OrderBy(x => dateTimeSelector(x)).ToList();
            var newList = new List<int?>();


            //var firstT = orderedList.First();

            //foreach (var t in orderedList.Skip(1))
            //{
            //    //if(t.)

            //}

            for (int i = 0; i < orderedList.Count(); i++)
            {
                int? value = GetValue(orderedList[i]);

                if (i > 0)
                {
                    value += GetCumSum(orderedList[i - 1]);
                    //newList.Add
                    SetCumSum(orderedList[i], value);
                }
                else
                {
                    if (GetCumSum(orderedList[i]) == null)
                    {
                        SetCumSum(orderedList[i], value);
                    }
                }

            }

        }
        
        public void CalculateCumSumFor<T>(
            IEnumerable<T> theList,
            Func<T, DateTime> dateTimeSelector,
            Func<T, Decimal?> GetValue,
            Func<T, Decimal?> GetCumSum,
            Action<T, Decimal?> SetCumSum
        ) where T : class
        {
            var orderedList = theList.OrderBy(x => dateTimeSelector(x)).ToList();

            for (int i = 0; i < orderedList.Count(); i++)
            {
                Decimal? value = GetValue(orderedList[i]);

                if (i > 0)
                {
                    value += GetCumSum(orderedList[i - 1]);
                }

                SetCumSum(orderedList[i], value);

                _healthContext.SaveChanges();
            }

        }



    }
}