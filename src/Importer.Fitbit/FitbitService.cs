using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repositories.Health.Models;

namespace Importer.Fitbit
{
    public class FitbitService : IFitbitService
    {
        private readonly IFitbitClientQueryAdapter _fitbitClientQueryAdapter;
        private readonly IFitbitClient _fitbitClient;
        private readonly IFitbitAuthenticator _fitbitAuthenticator;
        private readonly IFitbitMapper _fitbitMapper;

        public FitbitService(IFitbitClientQueryAdapter fitbitClientQueryAdapter, IFitbitClient fitbitClient, IFitbitAuthenticator fitbitAuthenticator, IFitbitMapper fitbitMapper)
        {
            _fitbitClientQueryAdapter = fitbitClientQueryAdapter;
            _fitbitClient = fitbitClient;
            _fitbitAuthenticator = fitbitAuthenticator;
            _fitbitMapper = fitbitMapper;
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