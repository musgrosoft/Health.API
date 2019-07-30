using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Repositories.Health.Models;
using Services.OAuth;
using Utils;

namespace Importer.Fitbit
{
    public class FitbitService : IFitbitService
    {
        private readonly FitbitClientQueryAdapter _fitbitClientQueryAdapter;
        private readonly FitbitAuthenticator _fitbitAuthenticator;
        private readonly FitbitMapper _fitbitMapper;

        public FitbitService(ITokenService tokenService, HttpClient httpClient, IConfig config, ILogger logger)
        {
            _fitbitClientQueryAdapter = new FitbitClientQueryAdapter(httpClient,config,logger);
            _fitbitAuthenticator =new FitbitAuthenticator(tokenService, httpClient, config, logger);
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