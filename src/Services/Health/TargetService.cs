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
        
        

        public List<StepCount> SetTargets(IList<StepCount> stepCounts)
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

        public List<ActivitySummary> SetTargets(IList<ActivitySummary> activitySummaries)
        {
            foreach (var activitySummary in activitySummaries)
            {
                activitySummary.TargetActiveMinutes = 30;
            }

            return activitySummaries.ToList();
        }

        public List<HeartRateSummary> SetTargets(IList<HeartRateSummary> heartRateSummaries)
        {
            foreach (var heartRateSummary in heartRateSummaries)
            {
                heartRateSummary.TargetCardioAndAbove = 11;
            }

            return heartRateSummaries.ToList();
        }

        public List<AlcoholIntake> SetTargets(IList<AlcoholIntake> alcoholIntakes)
        {
            foreach (var alcoholIntake in alcoholIntakes)
            {
                alcoholIntake.Target = 6;
            }

            return alcoholIntakes.ToList();
        }
    }
}
