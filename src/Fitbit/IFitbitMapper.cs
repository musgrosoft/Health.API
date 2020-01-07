using System.Collections.Generic;
using Fitbit.Domain;
using Repositories.Health.Models;

namespace Fitbit
{
    public interface IFitbitMapper
    {
        IEnumerable<RestingHeartRate> MapActivitiesHeartsToRestingHeartRates(IEnumerable<ActivitiesHeart> activitiesHearts);
        IEnumerable<SleepSummary> MapFitbitSleepsToSleepSummaries(IEnumerable<Domain.Sleep> sleeps);
    }
}