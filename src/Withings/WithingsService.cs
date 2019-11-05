using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace Withings
{
    public class WithingsService : IWithingsService
    {
        private readonly IWithingsAuthenticator _withingsAuthenticator;
        private readonly IWithingsMapper _withingsMapper;
        private readonly IWithingsClientQueryAdapter _withingsClientQueryAdapter;
        private readonly IWithingsClient _withingsClient;

        public WithingsService(IWithingsAuthenticator withingsAuthenticator, IWithingsMapper withingsMapper, IWithingsClientQueryAdapter withingsClientQueryAdapter, IWithingsClient withingsClient)
        {
            _withingsAuthenticator = withingsAuthenticator;
            _withingsMapper = withingsMapper;
            _withingsClientQueryAdapter = withingsClientQueryAdapter;
            _withingsClient = withingsClient;
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

        public async Task SetTokens(string authorizationCode)
        {
            await _withingsAuthenticator.SetTokens(authorizationCode);
        }
    }
}