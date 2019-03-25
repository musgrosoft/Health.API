using System.Collections.Generic;
using Fitbit.Domain;
using Repositories.Health.Models;

namespace Fitbit.Services
{
    public interface IFitbitMapper
    {
        IEnumerable<RestingHeartRate> MapActivitiesHeartsToRestingHeartRates(IEnumerable<ActivitiesHeart> activitiesHearts);

    }
}