using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace Importer.Fitbit
{
    public class FitbitService : IFitbitService
    {
        private readonly IFitbitClientQueryAdapter _fitbitClientQueryAdapter;
        private readonly IFitbitAuthenticator _fitbitAuthenticator;
        private readonly FitbitMapper _fitbitMapper;

        public FitbitService(IFitbitClientQueryAdapter fitbitClientQueryAdapter, IFitbitAuthenticator fitbitAuthenticator)
        {
            _fitbitClientQueryAdapter = fitbitClientQueryAdapter;
            _fitbitAuthenticator = fitbitAuthenticator;
            _fitbitMapper = new FitbitMapper();
        }
        
        public async Task<IEnumerable<RestingHeartRate>> GetRestingHeartRates(DateTime fromDate, DateTime toDate)
        {
            var accessToken = await _fitbitAuthenticator.GetAccessToken();

            var heartActivies = await _fitbitClientQueryAdapter.GetFitbitHeartActivities(fromDate, toDate, accessToken);

            return _fitbitMapper.MapActivitiesHeartsToRestingHeartRates(heartActivies);
        }

        public async Task<IEnumerable<MyFitbitSleep>> GetSleeps(DateTime fromDate, DateTime toDate)
        {
            var accessToken = await _fitbitAuthenticator.GetAccessToken();

            var sleeps = await _fitbitClientQueryAdapter.GetFitbitSleeps(fromDate, toDate, accessToken);

            var sl = _fitbitMapper.MapSleepsToFitbitSleeps(sleeps);

            return sl;
        }

        public async Task SetTokens(string code)
        {
            await _fitbitAuthenticator.SetTokens(code);
        }



    }
}