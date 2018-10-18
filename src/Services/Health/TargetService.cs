using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.Models;
using Utils;

namespace Services.Health
{
    public class TargetService : ITargetService
    {
        private readonly ITargetCalculator _targetCalculator;
        private readonly ICalendar _calendar;

        public TargetService(ITargetCalculator targetCalculator, ICalendar  calendar)
        {
            _targetCalculator = targetCalculator;
            _calendar = calendar;
        }

        public List<Weight> SetTargets(IList<Weight> weights)
        {
            if (weights.Any())
            {
                //var targetEndDate = _calendar.Now();//.AddDays(extraFutureDays);
                //var currentMaxDate = weights.Max(x => x.CreatedDate);
                //var futuredays = (targetEndDate - currentMaxDate).TotalDays;

                //for (int i = 1; i < futuredays + 1; i++)
                //{
                //    weights.Add(new Weight
                //    {
                //        CreatedDate = currentMaxDate.AddDays(i)
                //    });
                //}

                foreach (var weight in weights)
                {
                    weight.TargetKg = _targetCalculator.GetTargetWeight(weight.CreatedDate);
                }

                //weights = weights.OrderBy(x => x.CreatedDate).ToList();
            }

            return weights.ToList();
        }
        
        public IList<StepCount> SetTargets(List<StepCount> stepCounts)
        {
            foreach (var stepCount in stepCounts)
            {
                stepCount.TargetCumSumCount = _targetCalculator.GetTargetStepCountCumSum(stepCount.CreatedDate);
            }

            return stepCounts;
        }

        public IList<ActivitySummary> SetTargets(List<ActivitySummary> allActivitySummaries)
        {
            foreach (var activitySummary in allActivitySummaries)
            {
                activitySummary.TargetCumSumActiveMinutes = _targetCalculator.GetTargetActivitySummaryCumSum(activitySummary.CreatedDate);
            }

            return allActivitySummaries;
        }
        
        public IList<HeartRateSummary> SetTargets(List<HeartRateSummary> heartRateSummaries)
        {
            foreach (var heartRateSummary in heartRateSummaries)
            {
                heartRateSummary.TargetCumSumCardioAndAbove = _targetCalculator.GetTargetCumSumCardioAndAbove(heartRateSummary.CreatedDate);
            }

            return heartRateSummaries;
        }
        
        public IList<AlcoholIntake> SetTargets(List<AlcoholIntake> alcoholIntakes)
        {
            foreach (var alcoholIntake in alcoholIntakes)
            {
                alcoholIntake.TargetCumSumUnits = _targetCalculator.GetAlcoholIntakeTarget(alcoholIntake.CreatedDate);
            }

            return alcoholIntakes;
        }

        public List<StepCount> SetTargetsZZZ(IList<StepCount> stepCounts)
        {
            foreach (var stepCount in stepCounts)
            {
                stepCount.Target = 10000;
            }

            return stepCounts.ToList();
        }
    }
}
