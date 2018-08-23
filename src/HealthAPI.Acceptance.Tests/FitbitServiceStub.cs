using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Models;
using Services.Fitbit;

namespace HealthAPI.Acceptance.Tests
{
    public class FitbitServiceStub : IFitbitService
    {
        private bool _hasSubscribed = false;

        public bool HasSubscribed {
            get { return _hasSubscribed; }
        }

        public async Task<IEnumerable<ActivitySummary>> GetActivitySummaries(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<StepCount>> GetStepCounts(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<HeartRateSummary>> GetHeartSummaries(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RestingHeartRate>> GetRestingHeartRates(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public async Task Subscribe()
        {
            _hasSubscribed = true;
        }

        public async Task SetTokens(string code)
        {
            throw new NotImplementedException();
        }
    }
}