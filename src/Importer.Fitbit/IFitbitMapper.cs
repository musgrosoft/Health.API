using System.Collections.Generic;
using Importer.Fitbit.Domain;
using Repositories.Health.Models;

namespace Importer.Fitbit
{
    public interface IFitbitMapper
    {
        IEnumerable<RestingHeartRate> MapActivitiesHeartsToRestingHeartRates(IEnumerable<ActivitiesHeart> activitiesHearts);

        IEnumerable<FitbitSleep> MapSleepsToFitbitSleeps(IEnumerable<Sleep> sleeps);
    }
}