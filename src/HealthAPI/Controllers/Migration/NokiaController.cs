using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Migrators.Nokia;
using Services.Nokia;
using Utils;

namespace HealthAPI.Controllers.Migration
{
    [Produces("application/json")]
    [Route("api/Nokia")]
    public class NokiaController : Controller
    {
        private readonly ILogger _logger;
        private readonly INokiaMigrator _nokiaMigrator;
        private readonly INokiaService _nokiaService;

        private const int WeightKgMeasureTypeId = 1;
        private const int FatRatioPercentageMeasureTypeId = 6;
        private const int DiastolicBloodPressureMeasureTypeId = 9;
        private const int SystolicBloodPressureMeasureTypeId = 10;
        private const int SubscribeBloodPressureId = 4;


        public NokiaController(ILogger logger, INokiaMigrator nokiaMigrator, INokiaService nokiaService)
        {
            _logger = logger;
            _nokiaMigrator = nokiaMigrator;
            _nokiaService = nokiaService;
        }

        [HttpPost]
        [Route("Notify/Weights")]
        public async Task<IActionResult> MigrateWeights()
        {
            //http://www.yourdomain.net/yourCustomApplication.php ?userid=123456&startdate=1260350649 &enddate=1260350650&appli=44

            _logger.Log("NOKIA NEW : Migrating just weights");

            await _nokiaMigrator.MigrateWeights();

            _logger.Log("NOKIA : Finished Migrating just weights");

            return Ok();
        }

        [HttpPost]
        [Route("Notify/BloodPressures")]
        public async Task<IActionResult> MigrateBloodPressures()
        {
            //http://www.yourdomain.net/yourCustomApplication.php ?userid=123456&startdate=1260350649 &enddate=1260350650&appli=44

            _logger.Log("NOKIA NEW : Migrating just blood pressures");

            await _nokiaMigrator.MigrateBloodPressures();

            _logger.Log("NOKIA : Finished Migrating just blood pressures");

            return Ok();
        }

        [HttpGet]
        [Route("OAuth")]
        public async Task<IActionResult> OAuth([FromQuery]string code)
        {
            await _nokiaService.SetTokens(code);
            return Ok("Helllo123 change to useful message");
        }
        

        [HttpGet]
        [Route("Subscribe")]
        public async Task<IActionResult> Subscribe()
        {
            await _nokiaService.Subscribe();
            return Ok("Helllo123 change to useful message");
        }

        [HttpGet]
        [Route("ListSubscriptions")]
        public async Task<IActionResult> ListSubscriptions()
        {
            var subscriptions = await _nokiaService.GetSubscriptions();

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