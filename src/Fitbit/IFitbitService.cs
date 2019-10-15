using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace Fitbit
{
    public interface IFitbitService
    {
        Task<IEnumerable<RestingHeartRate>> GetRestingHeartRates(DateTime fromDate, DateTime toDate);
        Task SetTokens(string code);
        Task<IEnumerable<SleepSummary>> GetSleeps(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Drink>> GetDrinks(DateTime fromDate, DateTime toDate);
    }
}