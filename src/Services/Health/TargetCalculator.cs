using System;

namespace Services.Health
{
    public class TargetCalculator : ITargetCalculator
    {
        public Double? GetTargetWeight(DateTime dateTime)
        {
            var targetStartDate = new DateTime(2018, 5, 1);
            var targetEndDate = DateTime.Now.AddDays(600);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var weightOnTargetStartDate = 90.74;
            var targetDailyWeightLoss = 0.5 / 30;
            var targetDailyWeightLoss2 = 0.25 / 30;

            var daysToHitHealthyWeight = 123;

            var daysDiff = (dateTime - targetStartDate).TotalDays;

            if (daysDiff < 0)
            {
                return null;
            }

            if (daysDiff <= daysToHitHealthyWeight)
            {
                return weightOnTargetStartDate - (daysDiff * targetDailyWeightLoss);
            }

            if (daysDiff <= totalDays)
            {
                return weightOnTargetStartDate - (daysToHitHealthyWeight * targetDailyWeightLoss + (daysDiff - daysToHitHealthyWeight) * targetDailyWeightLoss2);
            }

            return null;
        }

        public int? GetTargetStepCount(DateTime dateTime)
        {
            var targetStartDate = new DateTime(2017, 5, 2);
            
            if ((dateTime - targetStartDate).TotalDays < 0)
            {
                return 0;
            }
            else
            {
                return 10000;
            }
        }

        public double? GetAlcoholIntakeTarget(DateTime dateTime)
        {
            var targetStartDate = new DateTime(2018, 5, 29);
            //var unitsOnTargetStartDate = 5148;
            var targetDailyUnits = 4;


            if (dateTime < targetStartDate)
            {
                return null;
            }

            return targetDailyUnits;
        }


        public int GetActivitySummaryTarget(DateTime dateTime)
        {
            return 30;
        }

        public int? GetCardioAndAboveTarget(DateTime heartSummaryCreatedDate)
        {
            return 11;
        }
    }
}