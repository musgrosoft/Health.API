using System;
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
    [Route("api/Fitbit")]
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
        [Route("Notification")]
        //public IActionResult Notify([FromBody] List<Note> notifications)
        public IActionResult Notify()
        {
            _logger.Log("Fitbit Notification : migrating all the things");

            BackgroundJob.Enqueue(() => MigrateAllTheThings());

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
            return Ok();
        }


        public async Task MigrateAllTheThings()
        {
            try
            {
                await _fitbitMigrator.MigrateAll();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

    }
}