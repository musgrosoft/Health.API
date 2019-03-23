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


    }
}