using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Fitbit.Domain;

namespace Services
{
    public interface IFitbitClientAggregator
    {
        Task<IEnumerable<FitbitDailyActivity>> GetFitbitDailyActivities(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<ActivitiesHeart>> GetFitbitHeartActivities(DateTime fromDate, DateTime toDate);
    }
}