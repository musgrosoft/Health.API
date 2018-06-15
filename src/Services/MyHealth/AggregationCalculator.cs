using System;
using System.Collections.Generic;
using System.Linq;
using Repositories;
using Repositories.Models;

namespace Services.MyHealth
{
    public class AggregationCalculator : IAggregationCalculator
    {
        
        public void SetMovingAveragesOnBloodPressures(IList<BloodPressure> seedBloodPressures, IList<BloodPressure> orderedBloodPressures, int period)
        {
            //_logger.Log("BLOOD PRESSURE : Add moving averages (using generic method)");

            SetMovingAveragesOn(
                seedBloodPressures,
                orderedBloodPressures,
                period,
                getValue: x => x.Systolic,
                setValue: (x, val) => x.MovingAverageSystolic = val
            );

            SetMovingAveragesOn(
                seedBloodPressures,
                orderedBloodPressures,
                period,
                getValue: x => x.Diastolic,
                setValue: (x, val) => x.MovingAverageDiastolic = val
            );


            //AddMovingAverageTo(
            //                bloodPressures,
            //                w => w.DateTime,
            //                w => w.Systolic,
            //                (w, d) => w.MovingAverageSystolic = d
            //                );

            //            AddMovingAverageTo(
            //                bloodPressures,
            //                w => w.DateTime,
            //                w => w.Diastolic,
            //                (w, d) => w.MovingAverageDiastolic = d
            //                );
        }

        //public void SetMovingAveragesOnRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates)
        //{
        //   // _logger.Log("RESTING HEART RATE : Add moving averages (using generic method)");

        //    AddMovingAverageTo(
        //        restingHeartRates,
        //        w => w.DateTime,
        //        w => w.Beats,
        //        (w, d) => w.MovingAverageBeats = d
        //        );
        //}

        public void SetMovingAveragesOnRestingHeartRates(IList<RestingHeartRate> seedRestingHeartRates, IList<RestingHeartRate> orderedRestingHeartRates, int period)
        {
            //AddMovingAverageTo(
            //    restingHeartRates,
            //    w => w.DateTime,
            //    w => w.Beats,
            //    (w, d) => w.MovingAverageBeats = d
            //);
            SetMovingAveragesOn(
                seedRestingHeartRates,
                orderedRestingHeartRates,
                period,
                getValue: x => x.Beats,
                setValue: (x, val) => x.MovingAverageBeats = val
            );

        }

        public void SetMovingAveragesOnWeights(IList<Weight> seedWeights, IList<Weight> orderedWeights, int period)
        {
            SetMovingAveragesOn(
                seedWeights,
                orderedWeights,
                period,
                getValue: x => x.Kg,
                setValue: (x, val) => x.MovingAverageKg = val
                );
        }

        
        private void SetMovingAveragesOn<T>(IList<T> seedTs, IList<T> orderedTs, int period, Func<T, Decimal?> getValue, Action<T, Decimal?> setValue)
        {
            if (seedTs.Count > period - 1)
            {
                seedTs = seedTs.TakeLast(period - 1).ToList();
            }

            var numberOfMissingKgs = period - 1 - seedTs.Count;

            List<T> allWeights = seedTs.ToList();
            allWeights.AddRange(orderedTs);

            for (int i = 0; i < orderedTs.Count; i++)
            {
                if (i < numberOfMissingKgs)
                {
                    setValue(orderedTs[i], null);
                    //                    orderedTs[i].MovingAverageKg = null;
                }
                else
                {
                    var average = allWeights.Skip(i - numberOfMissingKgs).Take(period).Average(x => getValue(x));
                    setValue(orderedTs[i], average);

                    //orderedTs[i].MovingAverageKg = allWeights.Skip(i - numberOfMissingKgs).Take(period).Average(x => x.Kg);
                }

            }

        }



        private void AddMovingAverageTo<T>(
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




















        public void SetCumSumsOnStepCounts(int? seed, IList<StepCount> orderedStepCounts)
        {
            for (int i = 0; i < orderedStepCounts.Count; i++)
            {
                int? previousCumSum;

                if (i == 0)
                {
                    previousCumSum = (seed ?? 0);
                }
                else
                {
                    previousCumSum = orderedStepCounts[i - 1].CumSumCount;
                }

                orderedStepCounts[i].CumSumCount = (orderedStepCounts[i].Count ?? 0) + previousCumSum;
            }
        }

        public void SetCumSumsOnActivitySummaries(int? seed, IList<ActivitySummary> orderedActivitySummaries)
        {
            for (int i = 0; i < orderedActivitySummaries.Count; i++)
            {
                int? previousCumSum;

                if (i == 0)
                {
                    previousCumSum = (seed ?? 0);
                }
                else
                {
                    previousCumSum = orderedActivitySummaries[i - 1].CumSumActiveMinutes;
                }

                orderedActivitySummaries[i].CumSumActiveMinutes = (orderedActivitySummaries[i].ActiveMinutes) + previousCumSum;
            }
        }

        public void SetCumSumsOnHeartSummaries(HeartSummary seedHeartSummary, IList<HeartSummary> orderedHeartSummaries)
        {
            for (int i = 0; i < orderedHeartSummaries.Count; i++)
            {
                int? previousCumSum;

                if (i == 0)
                {
                    previousCumSum = (seedHeartSummary?.CumSumCardioAndAbove ?? 0);
                }
                else
                {
                    previousCumSum = orderedHeartSummaries[i - 1].CumSumCardioAndAbove;
                }

                orderedHeartSummaries[i].CumSumCardioAndAbove = (orderedHeartSummaries[i].CardioMinutes) + (orderedHeartSummaries[i].PeakMinutes) + previousCumSum;
            }
        }


        public void SetCumSumsOnAlcoholIntakes(IList<AlcoholIntake> alcoholIntakes)
        {
            //  _logger.Log("UNITS : Calculate cum sum");

            CalculateCumSumFor(
                alcoholIntakes,
                ai => ai.DateTime,
                ai => ai.Units,
                ai => ai.CumSumUnits,
                (ai, val) => ai.CumSumUnits = val
            );
        }

        private void CalculateCumSumFor<T>(
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


            }

        }







    }
}