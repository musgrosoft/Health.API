﻿using Repositories.Health;
using Repositories.Models;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.MyHealth
{
    public class TargetService : ITargetService
    {


        //public List<TargetWeight> GetTargetWeights()
        //{

        //    var targetStartDate = new DateTime(2018, 5, 1);
        //    var targetEndDate = DateTime.Now.AddDays(600);
        //    var totalDays = (targetEndDate - targetStartDate).TotalDays;


        //    var allWeights = _healthRepository.GetWeightsFromDate(targetStartDate);
            
        //    var groups = allWeights.GroupBy(x => x.CreatedDate.Date);

        //    allWeights = groups.Select(x => new Weight
        //    {
        //        CreatedDate = x.Key.Date,
        //        Kg = x.Average(w => w.Kg),
        //        MovingAverageKg = x.Average(w => w.MovingAverageKg)
        //    }).ToList();

        //    var targetWeights = allWeights.Select(x => new TargetWeight
        //    {
        //        DateTime = x.CreatedDate,
        //        TargetKg = GetTargetWeight(x.CreatedDate),
        //        ActualKg = x.Kg,
        //        ActualMovingAverageKg = x.MovingAverageKg
        //    }).ToList();

        //    var futuredays = (targetEndDate - targetWeights.Min(x=>x.DateTime)).TotalDays;

        //    for (int i = 0; i < futuredays; i++)
        //    {
        //        var target = new TargetWeight
        //        {
        //            DateTime = DateTime.Now.AddDays(i),
        //            TargetKg = GetTargetWeight(DateTime.Now.AddDays(i))
        //        };

        //        targetWeights.Add(target);
        //    }

        //    targetWeights = targetWeights.OrderBy(x => x.DateTime).ToList();

        //    return targetWeights;
        //}

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



        private Double? GetTargetWeight(DateTime dateTime)
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
                stepCount.TargetCumSum = GetTargetStepCountCumSum(stepCount.CreatedDate);
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


        //[HttpGet]
        //public IActionResult StepCounts()
        //{
        //    var targets = new List<StepCount>();

        //    var targetStartDate = new DateTime(2017, 5, 3);
        //    var targetEndDate = DateTime.Now.AddDays(100);
        //    var totalDays = (targetEndDate - targetStartDate).TotalDays;

        //    var stepsOnTargetStartDate = 0;
        //    var targetDailySteps = 10000;

        //    for (var i = 0; i <= totalDays; i++)
        //    {
        //        var target = new StepCount()
        //        {
        //            CreatedDate = targetStartDate.AddDays(i),
        //            CumSumCount = stepsOnTargetStartDate + (i * targetDailySteps)
        //        };

        //        targets.Add(target);
        //    }

        //    return Json(targets);
        //}


    }
}
