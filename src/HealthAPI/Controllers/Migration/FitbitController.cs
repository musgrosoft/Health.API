using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Migrators;
using Services.Fitbit;
using Utils;

namespace HealthAPI.Controllers.Subscription
{
    [Produces("application/json")]
    [Route("api/Fitbit")]
    public class FitbitController : Controller
    {
        private readonly ILogger _logger;
        private readonly IConfig _config;
        private readonly IFitbitClient _fitbitClient;
        private readonly IFitbitMigrator _fitbitMigrator;

        public FitbitController(ILogger logger, IConfig config, IFitbitClient fitbitClient, IFitbitMigrator fitbitMigrator)
        {
            _logger = logger;
            _config = config;
            _fitbitClient = fitbitClient;
            _fitbitMigrator = fitbitMigrator;
        }

        [HttpPost]
        [Route("Notification")]
        public IActionResult Notify()
        {
            _logger.Log("Fitbit Notification : migrating all the things");

            BackgroundJob.Enqueue(() => _fitbitMigrator.MigrateAllTheThings());

            return (NoContent());
        }

        [HttpGet]
        [Route("Notification")]
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
            return Ok("useful message");
        }



        [HttpGet]
        [Route("OAuth")]
        public async Task<IActionResult> OAuth([FromQuery]string code)
        {
            
            await _fitbitClient.SetTokens(code);
            // await _nokiaAuthenticator.SetTokens(code);
            return Ok("Helllo123 change to useful message");
        }

    }
}