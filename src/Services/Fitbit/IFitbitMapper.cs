using System.Collections.Generic;
using Repositories.Models;

namespace Services.Fitbit.Domain
{
    public interface IFitbitMapper
    {
        IEnumerable<RestingHeartRate> MapActivitiesHeartToRestingHeartRates(IEnumerable<ActivitiesHeart> activitiesHearts);
        IEnumerable<HeartRateSummary> MapActivitiesHeartToHeartRateSummaries(IEnumerable<ActivitiesHeart> activitiesHearts);
        IEnumerable<ActivitySummary> MapFitbitDailyActivitiesToActivitySummaries(IEnumerable<FitbitDailyActivity> fitbitDailyActivities);
        IEnumerable<StepCount> MapToStepCounts(IEnumerable<FitbitDailyActivity> fitbitDailyActivities);
    }
}