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
        
        public double? GetTargetStepCountCumSum(DateTime dateTime)
        {
            var targetStartDate = new DateTime(2017, 5, 2);

            var stepsOnTargetStartDate = 0;
            var targetDailySteps = 10000;

            var daysDiff = (dateTime - targetStartDate).TotalDays;

            if (daysDiff < 0)
            {
                return null;
            }

            var days = (dateTime - targetStartDate).TotalDays;

            return stepsOnTargetStartDate + (days * targetDailySteps);
        }

        public double? GetTargetActivitySummaryCumSum(DateTime createdDate)
        {
            var targetStartDate = new DateTime(2017, 5, 2);
            var activeMinutesOnTargetStartDate = 0;
            var targetDailyActiveMinutes = 30;

            var daysDiff = (createdDate - targetStartDate).TotalDays;

            if (daysDiff < 0)
            {
                return null;
            }

            return (activeMinutesOnTargetStartDate + (daysDiff * targetDailyActiveMinutes));
        }

        public double? GetTargetCumSumCardioAndAbove(DateTime dateTime)
        {
            var targetStartDate = new DateTime(2018, 5, 20);
            var minutesOnTargetStartDate = 1775;
            var targetDailyMinutes = 11;

            var daysDiff = (dateTime - targetStartDate).TotalDays;

            if (daysDiff < 0)
            {
                return null;
            }

            return minutesOnTargetStartDate + (daysDiff * targetDailyMinutes);
        }

        public double? GetAlcoholIntakeTarget(DateTime dateTime)
        {
            var targetStartDate = new DateTime(2018, 5, 29);
            var unitsOnTargetStartDate = 5148;
            var targetDailyUnits = 4;

            var daysDiff = (dateTime - targetStartDate).TotalDays;

            if (daysDiff < 0)
            {
                return null;
            }

            return unitsOnTargetStartDate + (daysDiff * targetDailyUnits);
        }
    }
}