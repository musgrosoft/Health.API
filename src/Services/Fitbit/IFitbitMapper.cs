using System.Collections.Generic;
using Repositories.Models;

namespace Services.Fitbit.Domain
{
    public interface IFitbitMapper
    {
        IEnumerable<RestingHeartRate> MapActivitiesHeartsToRestingHeartRates(IEnumerable<ActivitiesHeart> activitiesHearts);
        IEnumerable<HeartRateSummary> MapActivitiesHeartsToHeartRateSummaries(IEnumerable<ActivitiesHeart> activitiesHearts);
        IEnumerable<ActivitySummary> MapFitbitDailyActivitiesToActivitySummaries(IEnumerable<FitbitDailyActivity> fitbitDailyActivities);
        IEnumerable<StepCount> MapFitbitDailyActivitiesToStepCounts(IEnumerable<FitbitDailyActivity> fitbitDailyActivities);
    }
}