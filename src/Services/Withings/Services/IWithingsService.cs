using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace Services.Withings.Services
{
    public interface IWithingsService
    {
        Task<IEnumerable<Weight>> GetWeights(DateTime sinceDateTime);
        Task<IEnumerable<BloodPressure>> GetBloodPressures(DateTime sinceDateTime);
        Task<List<string>> GetSubscriptions();
        Task Subscribe();
        Task SetTokens(string authorizationCode);
    }
}