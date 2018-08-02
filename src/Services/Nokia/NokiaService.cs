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

        public NokiaService(INokiaClient nokiaClient, INokiaAuthenticator nokiaAuthenticator)
        {
            _nokiaClient = nokiaClient;
            _nokiaAuthenticator = nokiaAuthenticator;
        }

        public async Task<IEnumerable<Weight>> GetWeights(DateTime sinceDateTime)
        {
            return await _nokiaClient.GetWeights(sinceDateTime);
        }

        public async Task<IEnumerable<BloodPressure>> GetBloodPressures(DateTime sinceDateTime)
        {
            return await _nokiaClient.GetBloodPressures(sinceDateTime);
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