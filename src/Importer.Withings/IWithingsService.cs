using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace Importer.Withings
{
    public interface IWithingsService
    {
        Task<IEnumerable<Weight>> GetWeights(DateTime sinceDateTime);
        Task<IEnumerable<BloodPressure>> GetBloodPressures(DateTime sinceDateTime);
        Task SetTokens(string authorizationCode);
        Task<IEnumerable<MyWithingsSleep>> GetSleeps(DateTime sinceDateTime);
    }
}