using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Models;

namespace Nokia.Services
{
    public class NokiaService : INokiaService
    {
        private readonly INokiaClient _nokiaClient;
        private readonly INokiaAuthenticator _nokiaAuthenticator;
        private readonly INokiaMapper _nokiaMapper;
        private readonly INokiaClientQueryAdapter _nokiaClientQueryAdapter;

        public NokiaService(INokiaClient nokiaClient, INokiaAuthenticator nokiaAuthenticator, INokiaMapper nokiaMapper, INokiaClientQueryAdapter nokiaClientQueryAdapter)
        {
            _nokiaClient = nokiaClient;
            _nokiaAuthenticator = nokiaAuthenticator;
            _nokiaMapper = nokiaMapper;
            _nokiaClientQueryAdapter = nokiaClientQueryAdapter;
        }

        public async Task<IEnumerable<Weight>> GetWeights(DateTime sinceDateTime)
        {
            var measureGroups = await _nokiaClientQueryAdapter.GetMeasureGroups(sinceDateTime);

            return _nokiaMapper.MapMeasuresGroupsToWeights(measureGroups);
        }

        public async Task<IEnumerable<BloodPressure>> GetBloodPressures(DateTime sinceDateTime)
        {
            var measureGroups = await _nokiaClientQueryAdapter.GetMeasureGroups(sinceDateTime);

            return _nokiaMapper.MapMeasuresGroupsToBloodPressures(measureGroups);
        }

        public async Task<List<string>> GetSubscriptions()
        {
            var weightSubscription = await _nokiaClient.GetWeightSubscription();
            var bloodPressureSubscription = await _nokiaClient.GetBloodPressureSubscription();

            var subscriptions = new List<string>
            {
                weightSubscription,
                bloodPressureSubscription
            };

            return subscriptions;
        }

        public async Task Subscribe()
        {
            await _nokiaClient.Subscribe();
        }

        public async Task SetTokens(string authorizationCode)
        {
            await _nokiaAuthenticator.SetTokens(authorizationCode);
        }
    }
}