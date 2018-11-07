using System;
using Google;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace HealthAPI.Controllers.Migration
{
    [Produces("application/json")]
    [Route("api/Google")]
    public class GoogleSheetsController : Controller
    {
        private readonly ILogger _logger;
        
        private readonly IGoogleImporter _googleImporter;

        public GoogleSheetsController(ILogger logger, IGoogleImporter googleImporter)
        {
            _logger = logger;
            _googleImporter = googleImporter;
        }
        
        [HttpGet]
        [Route("Notify/Runs")]
        //public async Task<IActionResult> Migrate()
        public IActionResult MigrateRuns()
        {
            _logger.LogMessageAsync("GOOGLE SHEETS : Migrate Runs");

            _googleImporter.MigrateRuns();

            return Ok();
        }

        [HttpGet]
        [Route("Notify/Ergos")]
        //public async Task<IActionResult> Migrate()
        public IActionResult MigrateErgos()
        {
            _logger.LogMessageAsync("GOOGLE SHEETS : Migrate Ergos");

            _googleImporter.MigrateErgos();

            return Ok();
        }

        [HttpGet]
        [Route("Notify/AlcoholIntakes")]
        //public async Task<IActionResult> Migrate()
        public IActionResult MigrateUnits()
        {
            _logger.LogMessageAsync("GOOGLE SHEETS : Migrate Units");

            _googleImporter.MigrateAlcoholIntakes();

            return Ok();
        }

    }

}