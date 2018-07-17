using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Migrators;
using Repositories;
using Repositories.Health;
using Repositories.Models;
using Repositories.OAuth;
using Services.Fitbit;
using Services.OAuth;
using Utils;

namespace HealthAPI.Controllers.Migration
{
    [Produces("application/json")]
    [Route("api/FitbitHeartRate")]
    public class FitbitDetailedHeartRateController : Controller
    {
        private ILogger _logger;
        private IOAuthService _oAuthService;
        private IFitbitMigrator _fitbitMigrator;

        public FitbitDetailedHeartRateController(
            ILogger logger,
            IOAuthService oAuthService,
            IFitbitMigrator fitbitMigrator)
        {

            _logger = logger;
            _oAuthService = oAuthService;
            _fitbitMigrator = fitbitMigrator;
        }

        //todo post
        [HttpGet]
        [Route("/{offset}")]
        public async Task<IActionResult> Migrate(int offset)
        {
            try
            {
                _logger.Log("FITBIT : starting fitbit migrate");

                var v = await _oAuthService.GetFitbitRefreshToken();

                var fitbitClient = new FitbitClient(new HttpClient(), new Config(),
                    new FitbitAuthenticator(
                        new OAuthService(new OAuthTokenRepository(new Config(), new Logger(new Config())))),
                    new Logger(new Config()));

                var repo = new HealthRepository(new HealthContext(new Config()));


                var date = DateTime.Now;



                var data = await fitbitClient.GetDetailedHeartRates(date.AddDays(-offset));

                foreach (var dataset in data)
                {
                    var heartRate = new HeartRate { CreatedDate = dataset.time, Bpm = dataset.value };


                    repo.Upsert(heartRate);
                }





                return Ok();

            }
            catch (Exception ex)
            {

                _logger.Error(ex);
                //todo return error
                return NotFound(ex.ToString());


            }

        }
    }
}