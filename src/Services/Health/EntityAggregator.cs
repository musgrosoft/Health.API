using System.Collections.Generic;
using System.Linq;
using Repositories.Models;
using Utils;

namespace Services.Health
{
    public class EntityAggregator : IEntityAggregator
    {

        public IList<HeartRateSummary> GroupByWeek(IList<HeartRateSummary> dailyHeartRateSummaries)
        {
            var weekGroups = dailyHeartRateSummaries.GroupBy(x => x.CreatedDate.GetWeekStartingOnMonday());

            var weeklyHeartRateSummaries = new List<HeartRateSummary>();
            foreach (var group in weekGroups)
            {
                var heartRateSummary = new HeartRateSummary
                {
                    CreatedDate = group.Key,
                    OutOfRangeMinutes = group.Sum(x => x.OutOfRangeMinutes),
                    FatBurnMinutes = group.Sum(x => x.FatBurnMinutes),
                    CardioMinutes = group.Sum(x => x.CardioMinutes),
                    PeakMinutes = group.Sum(x => x.PeakMinutes)

                };

                weeklyHeartRateSummaries.Add(heartRateSummary);
            }

            return weeklyHeartRateSummaries;
        }

        public IList<HeartRateSummary> GroupByMonth(IList<HeartRateSummary> dailyHeartRateSummaries)
        {
            var monthGroups = dailyHeartRateSummaries.GroupBy(x => x.CreatedDate.GetFirstDayOfMonth());
            
            var monthlyHeartRateSummaries = new List<HeartRateSummary>();
            foreach (var group in monthGroups)
            {
                var heartRateSummary = new HeartRateSummary
                {
                    CreatedDate = group.Key,
                    FatBurnMinutes = (int)group.Average(x => x.FatBurnMinutes),
                    CardioMinutes = (int)group.Average(x => x.CardioMinutes),
                    PeakMinutes = (int)group.Average(x => x.PeakMinutes)
                };

                monthlyHeartRateSummaries.Add(heartRateSummary);
            }

            return monthlyHeartRateSummaries;
        }

        public IList<StepCount> GroupByWeek(IList<StepCount> dailyStepCounts)
        {
            var weekGroups = dailyStepCounts.GroupBy(x => x.CreatedDate.GetWeekStartingOnMonday());

            var weeklyStepCounts = new List<StepCount>();
            foreach (var group in weekGroups)
            {
                var stepCount = new StepCount
                {
                    CreatedDate = group.Key,
                    Count = group.Sum(x => x.Count)
                };

                weeklyStepCounts.Add(stepCount);
            }

            return weeklyStepCounts;
        }

        public IList<StepCount> GroupByMonth(IList<StepCount> dailyStepCounts)
        {
            var monthGroups = dailyStepCounts.GroupBy(x => x.CreatedDate.GetFirstDayOfMonth());

            var monthlySteps = new List<StepCount>();

            foreach (var group in monthGroups)
            {
                var stepCount = new StepCount
                {
                    CreatedDate = group.Key,
                    Count = (int)group.Average(x => x.Count)
                };

                monthlySteps.Add(stepCount);
            }

            return monthlySteps;
        }

        public IList<AlcoholIntake> GroupByWeek(IList<AlcoholIntake> dailyAlcoholIntakes)
        {
            var weekGroups = dailyAlcoholIntakes.GroupBy(x => x.CreatedDate.GetWeekStartingOnMonday());

            var weeklyAlcoholIntakes = new List<AlcoholIntake>();
            foreach (var group in weekGroups)
            {
                var alcoholIntake = new AlcoholIntake
                {
                    CreatedDate = group.Key,
                    Units = group.Sum(x => x.Units)
                };

                weeklyAlcoholIntakes.Add(alcoholIntake);
            }

            return weeklyAlcoholIntakes;
        }

        public IList<AlcoholIntake> GroupByMonth(IList<AlcoholIntake> dailyAlcoholIntakes)
        {
            var monthGroups = dailyAlcoholIntakes.GroupBy(x => x.CreatedDate.GetFirstDayOfMonth());

            var monthlyUnits = new List<AlcoholIntake>();
            foreach (var group in monthGroups)
            {
                var alcoholIntake = new AlcoholIntake
                {
                    CreatedDate = group.Key,
                    Units = group.Average(x => x.Units)
                };

                monthlyUnits.Add(alcoholIntake);
            }

            return monthlyUnits;
        }


        public IList<ActivitySummary> GroupByWeek(IList<ActivitySummary> dailyActivities)
        {
            var weekGroups = dailyActivities.GroupBy(x => x.CreatedDate.GetWeekStartingOnMonday());

            var weeklyActivities = new List<ActivitySummary>();
            foreach (var group in weekGroups)
            {
                var activity = new ActivitySummary
                {
                    CreatedDate = group.Key,
                    //SedentaryMinutes = group.Sum(x => x.SedentaryMinutes),
                    //LightlyActiveMinutes = group.Sum(x => x.LightlyActiveMinutes),
                    FairlyActiveMinutes = group.Sum(x => x.FairlyActiveMinutes),
                    VeryActiveMinutes = group.Sum(x => x.VeryActiveMinutes)
                };

                weeklyActivities.Add(activity);
            }

            return weeklyActivities;
        }

        public IList<ActivitySummary> GroupByMonth(IList<ActivitySummary> dailyActivities)
        {
            var monthGroups = dailyActivities.GroupBy(x => x.CreatedDate.GetFirstDayOfMonth());
            
            var monthlyActivities = new List<ActivitySummary>();
            foreach (var group in monthGroups)
            {
                var activity = new ActivitySummary
                {
                    CreatedDate = group.Key,
                    //SedentaryMinutes = (int)group.Average(x => x.SedentaryMinutes),
                    //LightlyActiveMinutes = (int)group.Average(x => x.LightlyActiveMinutes),
                    FairlyActiveMinutes = (int)group.Average(x => x.FairlyActiveMinutes),
                    VeryActiveMinutes = (int)group.Average(x => x.VeryActiveMinutes)
                };

                monthlyActivities.Add(activity);
            }

            return monthlyActivities;
        }
    }
}