using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitbit.Domain;
using Repositories.Health.Models;

namespace Fitbit.Services
{
    public interface IFitbitClientQueryAdapter
    {
        Task<IEnumerable<FitbitDailyActivity>> GetFitbitDailyActivities(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<ActivitiesHeart>> GetFitbitHeartActivities(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Dataset>> GetDetailedHeartRates(DateTime fromDate, DateTime toDate);
    }
}