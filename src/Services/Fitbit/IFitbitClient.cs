using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Models;

namespace Services.Fitbit
{
    public interface IFitbitClient
    {
        Task<DailyActivity> GetDailyActivity(DateTime date);
        Task<IEnumerable<StepCount>> GetStepCounts(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<HeartRateZoneSummary>> GetMonthOfHeartZones(DateTime date);
        Task<IEnumerable<RestingHeartRate>> GetMonthOfRestingHeartRates(DateTime dateTime);
    }
}