using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.MyHealth
{
    public class TargetService : ITargetService
    {
        public IList<Weight> SetTargetWeights(IList<Weight> weights, int extraFutureDays)
        {
            var targetStartDate = new DateTime(2018, 5, 1);
            var targetEndDate = DateTime.Now.AddDays(extraFutureDays);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

            foreach (var weight in weights)
            {
                weight.TargetKg = GetTargetWeight(weight.CreatedDate);
            }

            var futuredays = (targetEndDate - weights.Max(x => x.CreatedDate)).TotalDays;

            for (int i = 0; i < futuredays; i++)
            {
                var target = new Weight
                {
                    CreatedDate = DateTime.Now.AddDays(i),
                    TargetKg = GetTargetWeight(DateTime.Now.AddDays(i))
                };

                weights.Add(target);
            }

            weights = weights.OrderBy(x => x.CreatedDate).ToList();

            return weights;
        }



        private static Double? GetTargetWeight(DateTime dateTime)
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



        public IList<StepCount> SetTargetStepCounts(List<StepCount> stepCounts, int extraFutureDays)
        {
            foreach (var stepCount in stepCounts)
            {
                stepCount.TargetCumSumCount = GetTargetStepCountCumSum(stepCount.CreatedDate);
            }

            return stepCounts;
        }

        private double? GetTargetStepCountCumSum(DateTime dateTime)
        {
            var targetStartDate = new DateTime(2017, 5, 3);
            //var targetEndDate = DateTime.Now.AddDays(100);
            //var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var stepsOnTargetStartDate = 0;
            var targetDailySteps = 10000;

            var days = (dateTime - targetStartDate).TotalDays;

            return stepsOnTargetStartDate + (days * targetDailySteps);
        }

        public IList<ActivitySummary> SetTargetActivitySummaries(List<ActivitySummary> allActivitySummaries, int extraFutureDays)
        {
            foreach (var activitySummary in allActivitySummaries)
            {
                activitySummary.TargetCumSumActiveMinutes = GetTargetActivitySummaryCumSum(activitySummary.CreatedDate);
            }

            return allActivitySummaries;
        }

        private double? GetTargetActivitySummaryCumSum(DateTime createdDate)
        {

            var targetStartDate = new DateTime(2017, 5, 2);
            var targetEndDate = DateTime.Now.AddDays(100);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var activeMinutesOnTargetStartDate = 0;
            var targetDailyActiveMinutes = 30;


            var daysDiff = (createdDate - targetStartDate).TotalDays;

            if (daysDiff < 0)
            {
                return null;
            }

            return (activeMinutesOnTargetStartDate + (daysDiff * targetDailyActiveMinutes));

        }

        public IList<HeartRateSummary> SetTargetHeartRateSummaries(List<HeartRateSummary> heartRateSummaries, int extraFutureDays)
        {
            foreach (var heartRateSummary in heartRateSummaries)
            {
                heartRateSummary.TargetCumSumCardioAndAbove = GetTargetActivitySummaryCumSum(heartRateSummary.CreatedDate);
            }

            return heartRateSummaries;
        }

        private double? GetTargetCumSumCardioAndAbove(DateTime dateTime)
        {

            var targetStartDate = new DateTime(2018, 5, 19);
            //var targetEndDate = DateTime.Now.AddDays(100);
            //var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var minutesOnTargetStartDate = 1775;
            var targetDailyMinutes = 11;

            var daysDiff = (dateTime - targetStartDate).TotalDays;

            if (daysDiff < 0)
            {
                return null;
            }

            return minutesOnTargetStartDate + (daysDiff * targetDailyMinutes);


        }

        public IList<AlcoholIntake> SetTargetAlcoholIntakes(List<AlcoholIntake> alcoholIntakes, int extraFutureDays)
        {
            foreach (var alcoholIntake in alcoholIntakes)
            {
                alcoholIntake.TargetCumSumUnits = GetTargetActivitySummaryCumSum(alcoholIntake.CreatedDate);
            }

            return alcoholIntakes;
        }

        private double? GetAlcoholIntakeTarget(DateTime dateTime)
        {
            var targetStartDate = new DateTime(2018, 5, 29);
            var targetEndDate = DateTime.Now.AddDays(100);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

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
