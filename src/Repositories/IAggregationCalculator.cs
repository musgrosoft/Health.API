using System;
using System.Collections.Generic;

namespace Repositories
{
    public interface IAggregationCalculator
    {
        void AddMovingAverageTo<T>(
            IEnumerable<T> theList,
            Func<T, DateTime> dateTimeSelector,
            Func<T, Decimal> GetValue,
            Action<T, Decimal?> SetMovingAverage,
            int period = 10
        ) where T : class;

        void CalculateCumSumFor<T>(
            IEnumerable<T> theList,
            Func<T, DateTime> dateTimeSelector,
            Func<T, int?> GetValue,
            Func<T, int?> GetCumSum,
            Action<T, int?> SetCumSum
        ) where T : class;

        void CalculateCumSumFor<T>(
            IEnumerable<T> theList,
            Func<T, DateTime> dateTimeSelector,
            Func<T, Decimal?> GetValue,
            Func<T, Decimal?> GetCumSum,
            Action<T, Decimal?> SetCumSum
        ) where T : class;
    }
}