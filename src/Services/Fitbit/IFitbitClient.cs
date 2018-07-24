using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Fitbit.Domain;
using Services.Fitbit.Domain.Detailed;

namespace Services.Fitbit
{
    public interface IFitbitClient
    {
        Task<FitbitDailyActivity> GetFitbitDailyActivity(DateTime date);
        Task<FitBitActivity> GetMonthOfFitbitActivities(DateTime startDate);
        Task<List<Dataset>> GetDetailedHeartRates(DateTime date);
        Task Subscribe();
    }
}