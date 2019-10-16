using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;
using Utils;

namespace Withings
{
    public class WithingsService : IWithingsService
    {
        private readonly IWithingsAuthenticator _withingsAuthenticator;
        private readonly IWithingsMapper _withingsMapper;
        private readonly IWithingsClientQueryAdapter _withingsClientQueryAdapter;
        private readonly IWithingsClient _withingsClient;
        private readonly IWithingsService _withingsService;

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

        public async Task<List<SleepState>> GetSleepStates()
        {

            var sleepStates = new List<SleepState>();

            for (int i = 0; i < 30; i++ )
            {
                var date = new DateTime(2019, 7, 18).AddDays(i);
                var accessToken = await _withingsAuthenticator.GetAccessToken();

                var d = await _withingsClient.Get1DayOfDetailedSleepData(date, accessToken);

                foreach (var sSeries in d)
                {
                    for (int j = sSeries.startdate; j < sSeries.enddate; j+=60)
                    {
                        var sleepState = new SleepState
                            {CreatedDate = j.ToDateFromUnixTime(), State = sSeries.state.ToString()};
                        sleepStates.Add(sleepState);
                    }
                }

            }

            return sleepStates;

        }

        public async Task SetTokens(string authorizationCode)
        {
            await _withingsAuthenticator.SetTokens(authorizationCode);
        }
    }
}