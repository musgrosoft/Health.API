using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Migrators;
using Newtonsoft.Json;
using Services.Fitbit;
using Utils;

namespace HealthAPI.Controllers.Subscription
{
    [Produces("application/json")]
    [Route("api/Fitbit/Notification")]
    public class FitbitSubscriptionController : Controller
    {
        private readonly ILogger _logger;
        private readonly IConfig _config;
        private readonly IFitbitClient _fitbitClient;
        private readonly IFitbitMigrator _fitbitMigrator;

        public FitbitSubscriptionController(ILogger logger, IConfig config, IFitbitClient fitbitClient, IFitbitMigrator fitbitMigrator)
        {
            _logger = logger;
            _config = config;
            _fitbitClient = fitbitClient;
            _fitbitMigrator = fitbitMigrator;
        }

        [HttpPost]
        // public IActionResult Notify([FromBody] List<Note> notifications)
        public IActionResult Notify()
        {
            _logger.Log("Fitbit Notification");

//            _logger.Log(notifications.ToString());
//            _logger.Log(JsonConvert.SerializeObject(notifications));

            BackgroundJob.Enqueue(() => MigrateAllTheThings());

            return (NoContent());
        }

        public async Task MigrateAllTheThings()
        {
            //var v = await _oAuthService.GetFitbitRefreshToken();

            //monthly gets
            await _fitbitMigrator.MigrateRestingHeartRates();
            await _fitbitMigrator.MigrateHeartSummaries();
            //daily gets
            await _fitbitMigrator.MigrateStepCounts();
            await _fitbitMigrator.MigrateActivitySummaries();


        }

        [HttpGet]
        public IActionResult Verify([FromQuery]string verify)
        {
            if (verify == _config.FitbitVerificationCode)
            {
                return (NoContent());
            }
            else
            {
                return (NotFound());
            }
        }

        [HttpGet]
        [Route("Subscribe")]
        public IActionResult Subscribe()
        {
            _fitbitClient.Subscribe();
            return Ok();
        }

    }
}