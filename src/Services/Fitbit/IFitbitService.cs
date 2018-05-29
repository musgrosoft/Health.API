using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Models;

namespace Services.Fitbit
{
    public interface IFitbitService
    {
        Task<IEnumerable<ActivitySummary>> GetActivitySummaries(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<StepCount>> GetStepCounts(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<HeartSummary>> GetHeartSummaries(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<RestingHeartRate>> GetRestingHeartRates(DateTime fromDate, DateTime toDate);
    }
}