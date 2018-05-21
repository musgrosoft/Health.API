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

            try
            {
                //var logger = context.Logger;
                var logger = new Logger();
                logger.Log("hello starting fitbit migrate");

                //logger.Log("STARTING NOKIA MIGRATOR");
                var healthService = HealthServiceFactory.Build( logger, _context);

                var oAuthService = new OAuthService(new OAuthTokenRepository(new Config(), logger));
                var v = await oAuthService.GetFitbitRefreshToken();
                logger.Log("fitbit refresh token is " + v);
              //  return Ok("fitbit refresh token is " + v);

                var fitbitAuthenticator = new FitbitAuthenticator(oAuthService);
                var fitbitAccessToken = await fitbitAuthenticator.GetAccessToken();

               // var healthService = HealthServiceFactory.Build(logger);

                var fitbitMigrator = new FitbitMigrator(healthService, logger, new FitbitService(new Config(), logger, new Calendar(), new System.Net.Http.HttpClient(), new FitbitClient(new System.Net.Http.HttpClient(), new Config(), fitbitAccessToken)));

                await fitbitMigrator.MigrateHeartZoneData();
                await fitbitMigrator.MigrateStepData();
                await fitbitMigrator.MigrateActivity();
                await fitbitMigrator.MigrateRestingHeartRateData();
                


                return Ok();
                //return new APIGatewayProxyResponse
                //{
                //    StatusCode = (int)HttpStatusCode.OK
                //};

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
                //LambdaLogger.Log(ex.ToString());

                //return new APIGatewayProxyResponse
                //{
                //    StatusCode = (int)HttpStatusCode.InternalServerError,
                //    Body = $"Uh oh, {ex.ToString()}"
                //};

            }
            return Ok();
        }

       
    }
}