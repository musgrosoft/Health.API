using System.Collections.Generic;
using Repositories.Health.Models;
using Services.Fitbit.Domain;

namespace Services.Fitbit.Services
{
    public interface IFitbitMapper
    {
        IEnumerable<RestingHeartRate> MapActivitiesHeartsToRestingHeartRates(IEnumerable<ActivitiesHeart> activitiesHearts);

    }
}