using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace Fitbit
{
    public class FitbitService : IFitbitService
    {
        private readonly IFitbitClientQueryAdapter _fitbitClientQueryAdapter;
        private readonly IFitbitAuthenticator _fitbitAuthenticator;
        private readonly IFitbitMapper _fitbitMapper;

        public FitbitService(IFitbitClientQueryAdapter fitbitClientQueryAdapter,IFitbitAuthenticator fitbitAuthenticator,  IFitbitMapper fitbitMapper)
        {
            _fitbitClientQueryAdapter = fitbitClientQueryAdapter;
            _fitbitAuthenticator = fitbitAuthenticator;
            _fitbitMapper = fitbitMapper;
        }
        
        public async Task<IEnumerable<RestingHeartRate>> GetRestingHeartRates(DateTime fromDate, DateTime toDate)
        {
            var accessToken = await _fitbitAuthenticator.GetAccessToken();

            var heartActivies = await _fitbitClientQueryAdapter.GetFitbitHeartActivities(fromDate, toDate, accessToken);

            return _fitbitMapper.MapActivitiesHeartsToRestingHeartRates(heartActivies);
        }

        public async Task<IEnumerable<SleepSummary>> GetSleepSummaries(DateTime fromDate, DateTime toDate)
        {
            var accessToken = await _fitbitAuthenticator.GetAccessToken();

            var fitbitSleeps = await _fitbitClientQueryAdapter.GetFitbitSleeps(fromDate, toDate, accessToken);

            var sleepSummaries = _fitbitMapper.MapFitbitSleepsToSleepSummaries(fitbitSleeps);

            return sleepSummaries;
        }
        
        public async Task SetTokens(string code)
        {
            await _fitbitAuthenticator.SetTokens(code);
        }



    }
}