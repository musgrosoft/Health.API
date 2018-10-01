using System;
using System.Threading.Tasks;
using Fitbit.Domain;

namespace Fitbit.Services
{
    public interface IFitbitClient
    {
        Task<FitbitDailyActivity> GetFitbitDailyActivity(DateTime date);
        Task<FitBitActivity> GetMonthOfFitbitActivities(DateTime startDate);
        Task Subscribe();

    }
}