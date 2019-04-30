using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace Importer.Withings
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
            var accessToken = await _withingsAuthenticator.GetAccessToken();

            var measureGroups = await _withingsClientQueryAdapter.GetMeasureGroups(sinceDateTime, accessToken);

            return _withingsMapper.MapToWeights(measureGroups);
        }

        public async Task<IEnumerable<BloodPressure>> GetBloodPressures(DateTime sinceDateTime)
        {
            var accessToken = await _withingsAuthenticator.GetAccessToken();

            var measureGroups = await _withingsClientQueryAdapter.GetMeasureGroups(sinceDateTime, accessToken);

            return _withingsMapper.MapToBloodPressures(measureGroups);
        }

        public async Task<List<string>> GetSubscriptions()
        {
            var accessToken = await _withingsAuthenticator.GetAccessToken();

            var weightSubscription = await _withingsClient.GetWeightSubscription(accessToken);
            var bloodPressureSubscription = await _withingsClient.GetBloodPressureSubscription(accessToken);

            var subscriptions = new List<string>
            {
                weightSubscription,
                bloodPressureSubscription
            };

            return subscriptions;
        }

        public async Task Subscribe()
        {
            var accessToken = await _withingsAuthenticator.GetAccessToken();
            await _withingsClient.Subscribe(accessToken);
        }

        public async Task SetTokens(string authorizationCode)
        {
            await _withingsAuthenticator.SetTokens(authorizationCode);
        }
    }
}