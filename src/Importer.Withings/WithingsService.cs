using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace Importer.Withings
{
    public class WithingsService : IWithingsService
    {
        private readonly IWithingsAuthenticator _withingsAuthenticator;
        private readonly IWithingsMapper _withingsMapper;
        private readonly IWithingsClientQueryAdapter _withingsClientQueryAdapter;

        public WithingsService(IWithingsAuthenticator withingsAuthenticator, IWithingsMapper withingsMapper, IWithingsClientQueryAdapter withingsClientQueryAdapter)
        {
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

        public async Task<IEnumerable<MyWithingsSleep>> GetSleeps(DateTime sinceDateTime)
        {
            var accessToken = await _withingsAuthenticator.GetAccessToken();

            var series = await _withingsClientQueryAdapter.GetSleepSeries(sinceDateTime, accessToken);

            return _withingsMapper.MapToMyWithingsSleep(series);
        }


        public async Task SetTokens(string authorizationCode)
        {
            await _withingsAuthenticator.SetTokens(authorizationCode);
        }
    }
}