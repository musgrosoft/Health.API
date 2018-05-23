using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Migrators;
using Repositories;
using Repositories.OAuth;
using Services.Fitbit;
using Services.MyHealth;
using Services.Nokia;
using Services.OAuth;
using Utils;

namespace HealthAPI.Controllers.Migration
{
    [Produces("application/json")]
    [Route("api/Fitbit")]
    public class FitbitController : Controller
    {
        private readonly HealthContext _context;

        public FitbitController(HealthContext context)
        {
            _context = context;
        }

        // POST: api/AlcoholIntakes1
        [HttpGet]
     //   [Route("api/Nokia/Migrate")]
        public async Task<IActionResult> Migrate()
        {

            var logger = new Logger();
            try
            {
                //var logger = context.Logger;
                logger.Log("FITBIT : starting fitbit migrate");

                //logger.Log("STARTING NOKIA MIGRATOR");
                var healthService = HealthServiceFactory.Build( logger, _context);

                var oAuthService = new OAuthService(new OAuthTokenRepository(new Config(), logger));
                var v = await oAuthService.GetFitbitRefreshToken();
                //logger.Log("fitbit refresh token is " + v);
              //  return Ok("fitbit refresh token is " + v);

                var fitbitAuthenticator = new FitbitAuthenticator(oAuthService);
                var fitbitAccessToken = await fitbitAuthenticator.GetAccessToken();

               // var healthService = HealthServiceFactory.Build(logger);

                var fitbitClient = new FitbitClient(new System.Net.Http.HttpClient(), new Config(), fitbitAccessToken, new Logger());
                var fitbitAggregator = new FitbitClientClientAggregator(fitbitClient);
                var fitbitService = new FitbitService(new Config(), logger, fitbitAggregator);

                var fitbitMigrator = new FitbitMigrator(healthService, logger, fitbitService, new Calendar());

                await fitbitMigrator.MigrateHeartSummaries();
                await fitbitMigrator.MigrateStepCounts();
                await fitbitMigrator.MigrateActivitySummaries();
                await fitbitMigrator.MigrateRestingHeartRates();

                logger.Log("FITBIT : finishing fitbit migrate");

                return Ok();
                //return new APIGatewayProxyResponse
                //{
                //    StatusCode = (int)HttpStatusCode.OK
                //};

            }
            catch (Exception ex)
            {
                
                logger.Error(ex);
                return NotFound(ex.ToString());
                //LambdaLogger.Log(ex.ToString());

                //return new APIGatewayProxyResponse
                //{
                //    StatusCode = (int)HttpStatusCode.InternalServerError,
                //    Body = $"Uh oh, {ex.ToString()}"
                //};

            }
        }

       
    }
}