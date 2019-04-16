using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace Services.Withings
{
    public class WithingsService : IWithingsService
    {
        private readonly IWithingsClient _withingsClient;
        private readonly IWithingsAuthenticator _withingsAuthenticator;
        private readonly IWithingsMapper _withingsMapper;
        private readonly IWithingsClientQueryAdapter _withingsClientQueryAdapter;

        public WithingsService(IWithingsClient withingsClient, IWithingsAuthenticator withingsAuthenticator, IWithingsMapper withingsMapper, IWithingsClientQueryAdapter withingsClientQueryAdapter)
        {
            _withingsClient = withingsClient;
            _withingsAuthenticator = withingsAuthenticator;
            _withingsMapper = withingsMapper;
            _withingsClientQueryAdapter = withingsClientQueryAdapter;
        }

        public async Task<IEnumerable<Weight>> GetWeights(DateTime sinceDateTime)
        {
            var measureGroups = await _withingsClientQueryAdapter.GetMeasureGroups(sinceDateTime);

            return _withingsMapper.MapMeasuresGroupsToWeights(measureGroups);
        }

        public async Task<IEnumerable<BloodPressure>> GetBloodPressures(DateTime sinceDateTime)
        {
            var measureGroups = await _withingsClientQueryAdapter.GetMeasureGroups(sinceDateTime);

            return _withingsMapper.MapMeasuresGroupsToBloodPressures(measureGroups);
        }

        public async Task<List<string>> GetSubscriptions()
        {
            var weightSubscription = await _withingsClient.GetWeightSubscription();
            var bloodPressureSubscription = await _withingsClient.GetBloodPressureSubscription();

            var subscriptions = new List<string>
            {
                weightSubscription,
                bloodPressureSubscription
            };

            return subscriptions;
        }

        public async Task Subscribe()
        {
            await _withingsClient.Subscribe();
        }

        public async Task SetTokens(string authorizationCode)
        {
            await _withingsAuthenticator.SetTokens(authorizationCode);
        }
    }
}