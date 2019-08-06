using System;
using System.Linq;
using System.Threading.Tasks;
using Importer.Withings;
using Microsoft.AspNetCore.Mvc;
using Services.Health;
using Utils;

namespace HealthAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Withings")]
    public class WithingsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IWithingsService _withingsService;
        private readonly IHealthService _healthService;
        
        private const int SEARCH_DAYS_PREVIOUS = 10;

        private DateTime MIN_WEIGHT_DATE = new DateTime(2012, 1, 1);
        private DateTime MIN_BLOOD_PRESSURE_DATE = new DateTime(2012, 1, 1);
        private DateTime MIN_SLEEP_DATE = new DateTime(2019, 7, 15);

        public WithingsController(ILogger logger, IWithingsService withingsService, IHealthService healthService)
        {
            _logger = logger;
            _withingsService = withingsService;
            _healthService = healthService;
        }

        [HttpPost]
        [Route("Notify/Weights")]
        public async Task<IActionResult> MigrateWeights()
        {
            var latestWeightDate = _healthService.GetLatestWeightDate(MIN_WEIGHT_DATE);
            var fromDate = latestWeightDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            var weights = (await _withingsService.GetWeights(fromDate)).ToList();

            _healthService.UpsertWeights(weights);

            await _logger.LogMessageAsync("Migrating just weights");
            await _logger.LogMessageAsync($"WEIGHT : Latest Weight record has a date of : {latestWeightDate:dd-MMM-yyyy HH:mm:ss (ddd)}");
            await _logger.LogMessageAsync($"WEIGHT : Found {weights.Count()} weight records, in previous {SEARCH_DAYS_PREVIOUS} days ");
            await _logger.LogMessageAsync("Finished Migrating just weights");

            return Ok();
        }

        [HttpPost]
        [Route("Notify/BloodPressures")]
        public async Task<IActionResult> MigrateBloodPressures()
        {
            var latestBloodPressureDate = _healthService.GetLatestBloodPressureDate(MIN_BLOOD_PRESSURE_DATE);
            var fromDate = latestBloodPressureDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            var bloodPressures = await _withingsService.GetBloodPressures(fromDate);
            _healthService.UpsertBloodpressures(bloodPressures);

            await _logger.LogMessageAsync($"BLOOD PRESSURE : Latest Blood Pressure record has a date of : {latestBloodPressureDate:dd-MMM-yyyy HH:mm:ss (ddd)} " +
                                          $"BLOOD PRESSURE : Retrieving Blood Pressure records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)} " +
                                          $"BLOOD PRESSURE : Upserting {bloodPressures.Count()} Blood Pressure records.");

            return Ok();
        }


        [HttpGet]
        [Route("OAuth")]
        public async Task<IActionResult> OAuth([FromQuery]string code)
        {
            await _withingsService.SetTokens(code);
            return Ok("Saved tokens successfully");
        }
       

    }
}