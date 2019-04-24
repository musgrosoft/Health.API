using System;
using Google;
using Importer.GoogleSheets;
using Microsoft.AspNetCore.Mvc;
using Services.Health;
using Utils;

namespace HealthAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Google")]
    public class GoogleSheetsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IGoogleClient _googleClient;
        private readonly IHealthService _healthService;
        
        public GoogleSheetsController(ILogger logger, IGoogleClient googleClient, IHealthService healthService)
        {
            _logger = logger;
            _googleClient = googleClient;
            _healthService = healthService;
            //_googleImporter = googleImporter;
        }


        [HttpGet]
        [Route("Notify/AlcoholIntakes")]
        //public async Task<IActionResult> Migrate()
        public IActionResult MigrateUnits()
        {
            _logger.LogMessageAsync("GOOGLE SHEETS : Migrate Units");

            //_googleImporter.MigrateAlcoholIntakes();

            var latestDrink = _healthService.GetLatestDrinkDate();

            if (latestDrink == DateTime.MinValue)
            {
                var historicAlcoholIntakes = _googleClient.GetHistoricAlcoholIntakes();
                _healthService.UpsertAlcoholIntakes(historicAlcoholIntakes);
            }

            var alcoholIntakes = _googleClient.GetAlcoholIntakes();
            _healthService.UpsertAlcoholIntakes(alcoholIntakes);

            return Ok();
        }

        [HttpGet]
        [Route("Notify/Exercises")]
        //public async Task<IActionResult> Migrate()
        public IActionResult ImportExercises()
        {
            _logger.LogMessageAsync("GOOGLE SHEETS : Import Exercises");

            //_googleImporter.ImportExercises();

            var rows = _googleClient.GetExercises();
            _healthService.UpsertExercises(rows);

            return Ok();
        }

    }

}