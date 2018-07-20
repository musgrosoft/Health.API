using System;
using Microsoft.AspNetCore.Mvc;
using Migrators;
using Services.MyHealth;
using Utils;

namespace HealthAPI.Controllers.Migration
{
    [Produces("application/json")]
    [Route("api/Google")]
    public class GoogleSheetsController : Controller
    {
        private readonly IHealthService _healthService;
        private readonly ILogger _logger;
        private readonly IConfig _config;
        private readonly IGoogleMigrator _googleMigrator;

        public GoogleSheetsController(IHealthService healthService, ILogger logger, IConfig config, IGoogleMigrator googleMigrator)
        {
            _healthService = healthService;
            _logger = logger;
            _config = config;
            _googleMigrator = googleMigrator;
        }

        [HttpGet]
        //public async Task<IActionResult> Migrate()
        public IActionResult Migrate()
        {

            try
            {
                //var logger = context.Logger;
                _logger.Log("GOOGLE SHEETS : starting google migrate");

                _googleMigrator.MigrateRuns();

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