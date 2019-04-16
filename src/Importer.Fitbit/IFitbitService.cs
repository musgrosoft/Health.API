using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace Services.Fitbit
{
    public interface IFitbitService
    {
        Task<IEnumerable<RestingHeartRate>> GetRestingHeartRates(DateTime fromDate, DateTime toDate);
        Task Subscribe();
        Task SetTokens(string code);
        
        
    }
}