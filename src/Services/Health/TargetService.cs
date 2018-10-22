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
            foreach (var weight in weights)
            {
                weight.TargetKg = _targetCalculator.GetTargetWeight(weight.CreatedDate);
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
            var targetStartDate = new DateTime(2017, 5, 2);

            foreach (var stepCount in stepCounts)
            {
                if ((stepCount.CreatedDate - targetStartDate).TotalDays < 0)
                {
                    stepCount.Target = 0;
                }
                else
                {
                    stepCount.Target = 10000;
                }

                
            }

            return stepCounts.ToList();
        }

        public List<ActivitySummary> SetTargetsZZZ(IList<ActivitySummary> activitySummaries)
        {
            foreach (var activitySummary in activitySummaries)
            {
                activitySummary.TargetActiveMinutes = 30;
            }

            return activitySummaries.ToList();
        }

        public List<HeartRateSummary> SetTargetsZZZ(IList<HeartRateSummary> heartRateSummaries)
        {
            foreach (var heartRateSummary in heartRateSummaries)
            {
                heartRateSummary.TargetCardioAndAbove = 11;
            }

            return heartRateSummaries.ToList();
        }

        public List<AlcoholIntake> SetTargetsZZZ(IList<AlcoholIntake> alcoholIntakes)
        {
            foreach (var alcoholIntake in alcoholIntakes)
            {
                alcoholIntake.Target = 6;
            }

            return alcoholIntakes.ToList();
        }
    }
}
