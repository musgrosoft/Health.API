using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Models;

namespace Services.Fitbit
{
    public interface IFitbitClient
    {
        Task<IEnumerable<DailyActivity>> GetDailyActivities(DateTime fromDate);
        Task<IEnumerable<StepCount>> GetStepCounts(DateTime fromDate);
        Task<IEnumerable<HeartRateZoneSummary>> GetHeartZones(DateTime fromDate);
        Task<IEnumerable<RestingHeartRate>> GetRestingHeartRates(DateTime fromDate);
    }
}