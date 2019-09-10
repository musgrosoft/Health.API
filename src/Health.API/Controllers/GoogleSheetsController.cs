using System;
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
        private readonly ISheetsService _sheetsService;
        private readonly IHealthService _healthService;
        
        public GoogleSheetsController(ILogger logger, ISheetsService sheetsService, IHealthService healthService)
        {
            _logger = logger;
            _sheetsService = sheetsService;
            _healthService = healthService;
        }
        
        //[HttpGet]
        //[Route("Notify/AlcoholIntakes")]
        //public IActionResult MigrateUnits()
        //{
        //    _logger.LogMessageAsync("GOOGLE SHEETS : Migrate Units");

        //    var latestDrink = _healthService.GetLatestDrinkDate();

        //    //if (latestDrink == DateTime.MinValue)
        //    //{
        //        var historicAlcoholIntakes = _sheetsService.GetHistoricDrinks();
        //        _healthService.UpsertAlcoholIntakes(historicAlcoholIntakes);
        //    //}

        //    var alcoholIntakes = _sheetsService.GetDrinks();
        //    _healthService.UpsertAlcoholIntakes(alcoholIntakes);

        //    return Ok();
        //}

        [HttpGet]
        [Route("Notify/Exercises")]
        public IActionResult ImportExercises()
        {
            _logger.LogMessageAsync("GOOGLE SHEETS : Import Exercises");
            
            var rows = _sheetsService.GetExercises();
            _healthService.UpsertExercises(rows);

            return Ok();
        }

    }

}