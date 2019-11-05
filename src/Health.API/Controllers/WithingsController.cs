using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Health;
using Utils;
using Withings;

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
        //private DateTime MIN_SLEEP_DATE = new DateTime(2019, 7, 15);

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
            await _logger.LogMessageAsync("WEIGHTS : NOTIFICATION from Withings");

            var latestWeightDate = _healthService.GetLatestWeightDate(MIN_WEIGHT_DATE);
            var fromDate = latestWeightDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            await _logger.LogMessageAsync($"WEIGHTS : Latest record has a date of : {latestWeightDate:dd-MMM-yyyy HH:mm:ss (ddd)}, will retrieve from {SEARCH_DAYS_PREVIOUS} days previous to this date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}.");
                        
            var weights = (await _withingsService.GetWeights(fromDate)).ToList();

            await _logger.LogMessageAsync($"WEIGHTS : Found {weights.Count()} records.");

            if (weights.Any())
            {
                await _logger.LogMessageAsync($"WEIGHTS : First at {weights.Min(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)} , last at {weights.Max(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)}.");
            }

            _healthService.UpsertWeights(weights);       
            
            await _logger.LogMessageAsync("WEIGHTS: Finished Importing weights.");
                       
            return Ok();
        }

        [HttpPost]
        [Route("Notify/BloodPressures")]
        public async Task<IActionResult> MigrateBloodPressures()
        {
            await _logger.LogMessageAsync("BLOOD PRESSURES : NOTIFICATION from Withings");

            var latestBloodPressureDate = _healthService.GetLatestBloodPressureDate(MIN_BLOOD_PRESSURE_DATE);
            var fromDate = latestBloodPressureDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            await _logger.LogMessageAsync($"BLOOD PRESSURES : Latest record has a date of : {latestBloodPressureDate:dd-MMM-yyyy HH:mm:ss (ddd)}, will retrieve from {SEARCH_DAYS_PREVIOUS} days previous to this date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}.");

            var bloodPressures = await _withingsService.GetBloodPressures(fromDate);

            await _logger.LogMessageAsync($"BLOOD PRESSURES : Found {bloodPressures.Count()} records.");

            if (bloodPressures.Any())
            {
                await _logger.LogMessageAsync($"BLOOD PRESSURES : First at {bloodPressures.Min(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)} , last at {bloodPressures.Max(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)}.");
            }

            _healthService.UpsertBloodPressures(bloodPressures);

            await _logger.LogMessageAsync("BLOOD PRESSURES: Finished Importing.");

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