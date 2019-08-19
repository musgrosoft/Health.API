using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Importer.Fitbit.Internal;
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

        public async Task<IEnumerable<Sleep>> GetSleeps(DateTime fromDate, DateTime toDate)
        {
            var accessToken = await _fitbitAuthenticator.GetAccessToken();

            var fitbitSleeps = await _fitbitClientQueryAdapter.GetFitbitSleeps(fromDate, toDate, accessToken);

            var sleeps = _fitbitMapper.MapSleepsToFitbitSleeps(fitbitSleeps);

            return sleeps;
        }

        public async Task<IEnumerable<Drink>> GetDrinks(DateTime fromDate, DateTime toDate)
        {
            var accessToken = await _fitbitAuthenticator.GetAccessToken();

            var foods = await _fitbitClientQueryAdapter.GetFitbitFoods(fromDate, toDate, accessToken);

            var drinks = _fitbitMapper.MapFitbitFoodsToDrinks(foods);

            return drinks;
        }

        public async Task SetTokens(string code)
        {
            await _fitbitAuthenticator.SetTokens(code);
        }



    }
}