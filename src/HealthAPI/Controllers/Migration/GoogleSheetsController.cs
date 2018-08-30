using System;
using Microsoft.AspNetCore.Mvc;
using Migrators.Google;
using Utils;

namespace HealthAPI.Controllers.Migration
{
    [Produces("application/json")]
    [Route("api/Google")]
    public class GoogleSheetsController : Controller
    {
        private readonly ILogger _logger;
        
        private readonly IGoogleMigrator _googleMigrator;

        public GoogleSheetsController(ILogger logger, IGoogleMigrator googleMigrator)
        {
            _logger = logger;
            _googleMigrator = googleMigrator;
        }
        
        [HttpGet]
        [Route("Notify/Runs")]
        //public async Task<IActionResult> Migrate()
        public IActionResult MigrateRuns()
        {
            _logger.Log("GOOGLE SHEETS : Migrate Runs");

            _googleMigrator.MigrateRuns();

            return Ok();
        }

        [HttpGet]
        [Route("Notify/Ergos")]
        //public async Task<IActionResult> Migrate()
        public IActionResult MigrateErgos()
        {
            _logger.Log("GOOGLE SHEETS : Migrate Ergos");

            _googleMigrator.MigrateErgos();

            return Ok();
        }

        [HttpGet]
        [Route("Notify/AlcoholIntakes")]
        //public async Task<IActionResult> Migrate()
        public IActionResult MigrateUnits()
        {
            _logger.Log("GOOGLE SHEETS : Migrate Units");

            _googleMigrator.MigrateAlcoholIntakes();

            return Ok();
        }

    }

}