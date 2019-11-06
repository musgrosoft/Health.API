using System;
using System.Linq;
using System.Threading.Tasks;
using GoogleSheets;
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

        private const int SEARCH_DAYS_PREVIOUS = 10;

        private DateTime MIN_DRINK_DATE = new DateTime(2016, 1, 1);
        private DateTime MIN_EXERCISEE_DATE = new DateTime(2017, 5, 3);

        public GoogleSheetsController(ILogger logger, ISheetsService sheetsService, IHealthService healthService)
        {
            _logger = logger;
            _sheetsService = sheetsService;
            _healthService = healthService;
        }
        
        [HttpGet]
        [Route("Notify/AlcoholIntakes")]
        public async Task<IActionResult> MigrateUnits()
        {
            await _logger.LogMessageAsync("DRINKS : Notification (from Google Sheets)");

            var latestDrinkDate = _healthService.GetLatestDrinkDate(MIN_DRINK_DATE);
            var fromDate = latestDrinkDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            await _logger.LogMessageAsync($"DRINKS : Latest record has a date of : {latestDrinkDate:dd-MMM-yyyy HH:mm:ss (ddd)}, will retrieve from {SEARCH_DAYS_PREVIOUS} days previous to this date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}.");

            var drinks = _sheetsService.GetDrinks(fromDate);

            await _logger.LogMessageAsync($"DRINKS : Found {drinks.Count()} records.");

            if (drinks.Any())
            {
                await _logger.LogMessageAsync($"DRINKS : First at {drinks.Min(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)} , last at {drinks.Max(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)}.");
            }

            _healthService.UpsertAlcoholIntakes(drinks);

            await _logger.LogMessageAsync("DRINKS: Finished Importing.");

            return Ok();
        }

        [HttpGet]
        [Route("Notify/Exercises")]
        public async Task<IActionResult> ImportExercises()
        {
            await _logger.LogMessageAsync("EXERCISES : Notificattion (from Google Sheets).");

            var latestExerciseDate = _healthService.GetLatestExerciseDate(MIN_EXERCISEE_DATE);
            var fromDate = latestExerciseDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            await _logger.LogMessageAsync($"EXERCISES : Latest record has a date of : {latestExerciseDate:dd-MMM-yyyy HH:mm:ss (ddd)}, will retrieve from {SEARCH_DAYS_PREVIOUS} days previous to this date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}.");

            var exercises = _sheetsService.GetExercises();

            await _logger.LogMessageAsync($"EXERCISES : Found {exercises.Count()} records.");

            if (exercises.Any())
            {
                await _logger.LogMessageAsync($"EXERCISES : First at {exercises.Min(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)} , last at {exercises.Max(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)}.");
            }

            _healthService.UpsertExercises(exercises);


            await _logger.LogMessageAsync("EXERCISES: Finished Importing.");

            return Ok();
        }

    }

}