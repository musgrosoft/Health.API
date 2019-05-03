using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace Importer.Fitbit
{
    public interface IFitbitService
    {
        Task<IEnumerable<RestingHeartRate>> GetRestingHeartRates(DateTime fromDate, DateTime toDate);
        Task SetTokens(string code);
    }
}