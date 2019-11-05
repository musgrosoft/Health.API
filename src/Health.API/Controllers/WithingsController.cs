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
            await _logger.LogMessageAsync($"WEIGHTS : Latest Weight record has a date of : {latestWeightDate:dd-MMM-yyyy HH:mm:ss (ddd)}, will retrieve from {SEARCH_DAYS_PREVIOUS} days previous to this date");

            var fromDate = latestWeightDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            var weights = (await _withingsService.GetWeights(fromDate)).ToList();
            await _logger.LogMessageAsync($"WEIGHTS : Found {weights.Count()} weight records from {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            _healthService.UpsertWeights(weights);       
            
            await _logger.LogMessageAsync("WEIGHTS: Finished Importing weights");



            //var sleepStates = await _withingsService.GetSleepStates();
            //sleepStates = sleepStates.Select(x => new Repositories.Health.Models.SleepState
            //{
            //    CreatedDate = x.CreatedDate.AddYears(-1),
            //    State = x.State
            //}).ToList();


            //_healthService.UpsertSleepStates(sleepStates);


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