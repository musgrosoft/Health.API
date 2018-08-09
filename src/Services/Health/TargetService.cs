using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.Models;

namespace Services.Health
{
    public class TargetService : ITargetService
    {
        private readonly ITargetCalculator _targetCalculator;

        public TargetService(ITargetCalculator targetCalculator)
        {
            _targetCalculator = targetCalculator;
        }

        public IList<Weight> SetTargets(IList<Weight> weights, int extraFutureDays)
        {
            var targetEndDate = DateTime.Now.AddDays(extraFutureDays);
            var futuredays = (targetEndDate - weights.Max(x => x.CreatedDate)).TotalDays;

            for (int i = 0; i < futuredays; i++)
            {
                weights.Add(new Weight
                {
                    CreatedDate = DateTime.Now.AddDays(i)
                });
            }

            foreach (var weight in weights)
            {
                weight.TargetKg = _targetCalculator.GetTargetWeight(weight.CreatedDate);
            }

            weights = weights.OrderBy(x => x.CreatedDate).ToList();

            return weights;
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


    }
}
