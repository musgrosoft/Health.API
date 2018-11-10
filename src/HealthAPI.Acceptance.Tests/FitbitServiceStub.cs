using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fitbit.Services;
using Repositories.Health.Models;

namespace HealthAPI.Acceptance.Tests
{
    public class FitbitServiceStub : IFitbitService
    {

        public bool HasSubscribed { get; private set; } = false;

        public Task<IEnumerable<ActivitySummary>> GetActivitySummaries(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<StepCount>> GetStepCounts(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HeartRateSummary>> GetHeartSummaries(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RestingHeartRate>> GetRestingHeartRates(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task Subscribe()
        {
            HasSubscribed = true;

            return Task.CompletedTask;
        }

        public Task SetTokens(string code)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HeartRate>> GetDetailedHeartRates(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Run>> GetRuns(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HeartRate>> GetDetailedHeartRates(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }
    }
}