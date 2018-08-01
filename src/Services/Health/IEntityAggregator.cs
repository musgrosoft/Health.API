using System.Collections.Generic;
using Repositories.Models;

namespace Services.Health
{
    public interface IEntityAggregator
    {
        IList<ActivitySummary> GroupByWeek(IList<ActivitySummary> dailyActivities);
        IList<ActivitySummary> GroupByMonth(IList<ActivitySummary> dailyActivities);
        IList<HeartRateSummary> GroupByWeek(IList<HeartRateSummary> dailyHeartRateSummaries);
        IList<HeartRateSummary> GroupByMonth(IList<HeartRateSummary> dailyHeartRateSummaries);

        IList<StepCount> GroupByWeek(IList<StepCount> stepCounts);
        IList<StepCount> GroupByMonth(IList<StepCount> stepCounts);

        IList<AlcoholIntake> GroupByWeek(IList<AlcoholIntake> alcoholIntakes);
        IList<AlcoholIntake> GroupByMonth(IList<AlcoholIntake> alcoholIntakes);
    }
}