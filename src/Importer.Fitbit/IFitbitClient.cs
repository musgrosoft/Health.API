using System;
using System.Threading.Tasks;
using Importer.Fitbit.Domain;

namespace Importer.Fitbit
{
    public interface IFitbitClient
    {
        //Task<FitbitDailyActivity> GetFitbitDailyActivity(DateTime date);
        Task<FitBitActivity> GetMonthOfFitbitActivities(DateTime startDate);
        Task Subscribe();

        //Task<List<Dataset>> GetDetailedHeartRates(DateTime date);
    }
}