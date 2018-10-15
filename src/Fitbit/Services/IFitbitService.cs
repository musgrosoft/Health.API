using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Models;

namespace Fitbit.Services
{
    public interface IFitbitService
    {
        Task<IEnumerable<ActivitySummary>> GetActivitySummaries(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<StepCount>> GetStepCounts(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<HeartRateSummary>> GetHeartSummaries(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<RestingHeartRate>> GetRestingHeartRates(DateTime fromDate, DateTime toDate);
        Task Subscribe();
        Task SetTokens(string code);
        Task<IEnumerable<Run>> GetRuns(DateTime fromDate, DateTime toDate);
    }
}