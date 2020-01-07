using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitbit.Domain;

namespace Fitbit
{
    public interface IFitbitClientQueryAdapter
    {
        Task<IEnumerable<ActivitiesHeart>> GetFitbitHeartActivities(DateTime fromDate, DateTime toDate, string accessToken);
        Task<IEnumerable<Sleep>> GetFitbitSleeps(DateTime fromDate, DateTime toDate, string accessToken);
    }
}