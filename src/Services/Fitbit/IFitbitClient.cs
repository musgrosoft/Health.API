using System;
using System.Threading.Tasks;
using Services.Fitbit.Domain;

namespace Services.Fitbit
{
    public interface IFitbitClient
    {
        //Task<FitbitDailyActivity> GetFitbitDailyActivity(DateTime date);
        Task<FitBitActivity> GetMonthOfFitbitActivities(DateTime startDate);
        Task Subscribe();

        //Task<List<Dataset>> GetDetailedHeartRates(DateTime date);
    }
}