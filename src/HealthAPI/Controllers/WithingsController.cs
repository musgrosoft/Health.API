using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Health;
using Services.Withings;
using Utils;

namespace HealthAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Nokia")]
    public class WithingsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IWithingsService _withingsService;
        private readonly IHealthService _healthService;

        //private const int WeightKgMeasureTypeId = 1;
        //private const int FatRatioPercentageMeasureTypeId = 6;
        //private const int DiastolicBloodPressureMeasureTypeId = 9;
        //private const int SystolicBloodPressureMeasureTypeId = 10;
        //private const int SubscribeBloodPressureId = 4;


        private const int SEARCH_DAYS_PREVIOUS = 10;

        private DateTime MIN_WEIGHT_DATE = new DateTime(2012, 1, 1);
        private DateTime MIN_BLOOD_PRESSURE_DATE = new DateTime(2012, 1, 1);

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
            //http://www.yourdomain.net/yourCustomApplication.php ?userid=123456&startdate=1260350649 &enddate=1260350650&appli=44

            await _logger.LogMessageAsync("NOKIA NEW : Migrating just weights");

            //await _withingsImporter.MigrateWeights();

            var latestWeightDate = _healthService.GetLatestWeightDate(MIN_WEIGHT_DATE);
            await _logger.LogMessageAsync($"WEIGHT : Latest Weight record has a date of : {latestWeightDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var fromDate = latestWeightDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            var weights = (await _withingsService.GetWeights(fromDate)).ToList();
            await _logger.LogMessageAsync($"WEIGHT : Found {weights.Count()} weight records, in previous {SEARCH_DAYS_PREVIOUS} days ");

            _healthService.UpsertWeights(weights);

            await _logger.LogMessageAsync("NOKIA : Finished Migrating just weights");

            return Ok();
        }

        [HttpPost]
        [Route("Notify/BloodPressures")]
        public async Task<IActionResult> MigrateBloodPressures()
        {
            //http://www.yourdomain.net/yourCustomApplication.php ?userid=123456&startdate=1260350649 &enddate=1260350650&appli=44

            await _logger.LogMessageAsync("NOKIA NEW : Migrating just blood pressures");

            //await _withingsImporter.MigrateBloodPressures();

            var latestBloodPressureDate = _healthService.GetLatestBloodPressureDate(MIN_BLOOD_PRESSURE_DATE);
            await _logger.LogMessageAsync($"BLOOD PRESSURE : Latest Blood Pressure record has a date of : {latestBloodPressureDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var fromDate = latestBloodPressureDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            await _logger.LogMessageAsync($"BLOOD PRESSURE : Retrieving Blood Pressure records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var bloodPressures = await _withingsService.GetBloodPressures(fromDate);
            await _logger.LogMessageAsync($"BLOOD PRESSURE : Found {bloodPressures.Count()} Blood Pressure records.");

            _healthService.UpsertBloodPressures(bloodPressures);


            await _logger.LogMessageAsync("NOKIA : Finished Migrating just blood pressures");

            return Ok();
        }

        [HttpGet]
        [Route("OAuth")]
        public async Task<IActionResult> OAuth([FromQuery]string code)
        {
            await _withingsService.SetTokens(code);
            return Ok("Helllo123 change to useful message");
        }
        

        [HttpGet]
        [Route("Subscribe")]
        public async Task<IActionResult> Subscribe()
        {
            await _withingsService.Subscribe();
            return Ok("Helllo123 change to useful message");
        }

        [HttpGet]
        [Route("ListSubscriptions")]
        public async Task<IActionResult> ListSubscriptions()
        {
            var subscriptions = await _withingsService.GetSubscriptions();

            return Ok(subscriptions);
        }

        
        [HttpGet]
        [ProducesResponseType(typeof(String), 200)]
        [Route("OAuthUrl")]
        public IActionResult OAuthUrl()
        {
            var redirectUrl = "http://musgrosoft-health-api.azurewebsites.net/api/fitbit/oauth/";

            return Json("https://account.health.nokia.com/oauth2_user/authorize2?response_type=code&redirect_uri=http://musgrosoft-health-api.azurewebsites.net/api/nokia/oauth/&client_id=09d4e17f36ee237455246942602624feaad12ac51598859bc79ddbd821147942&scope=user.info,user.metrics,user.activity&state=768uyFys");
        }
        

    }
}