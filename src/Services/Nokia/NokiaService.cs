using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Models;

namespace Services.Nokia
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

        public async Task<string> GetSubscriptions()
        {
            return await _nokiaClient.GetSubscriptions();
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