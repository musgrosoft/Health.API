using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Migrators;
using Migrators.Hangfire;
using Services.Fitbit;
using Utils;

namespace HealthAPI.Controllers.Subscription
{
    [Produces("application/json")]
    [Route("api/Fitbit")]
    public class FitbitController : Controller
    {
        private readonly ILogger _logger;
        private readonly IConfig _config;
        private readonly IFitbitService _fitbitService;
        private readonly IHangfireUtility _hangfireUtility;
        private readonly IHangfireWork _hangfireWork;

        public FitbitController(ILogger logger, IConfig config, IFitbitService fitbitService, IHangfireUtility hangfireUtility, IHangfireWork hangfireWork)
        {
            _logger = logger;
            _config = config;
            _fitbitService = fitbitService;
            _hangfireUtility = hangfireUtility;
            _hangfireWork = hangfireWork;
        }

        [HttpPost]
        [Route("Notification")]
        public IActionResult Notify()
        {
            _logger.Log("Fitbit Notification : MIGRATING ALL FITBIT DATA");

            _hangfireUtility.Enqueue(() => _hangfireWork.MigrateAllFitbitData());

            return (NoContent());
        }

        [HttpGet]
        [Route("Notification")]
        public IActionResult Verify([FromQuery]string verify)
        {
            if (verify == _config.FitbitVerificationCode)
            {
                return (NoContent());
            }
            else
            {
                return (NotFound());
            }
        }

        [HttpGet]
        [Route("Subscribe")]
        public IActionResult Subscribe()
        {
            _fitbitService.Subscribe();
            return Ok("useful message");
        }

        [HttpGet]
        [Route("OAuth")]
        public async Task<IActionResult> OAuth([FromQuery]string code)
        {
            
            await _fitbitService.SetTokens(code);
            return Ok("Helllo123 change to useful message");
        }

        [HttpGet]
        [ProducesResponseType(typeof(String), 200)]
        [Route("OAuthUrl")]
        public IActionResult OAuthUrl()
        {
            var redirectUrl = "http://musgrosoft-health-api.azurewebsites.net/api/fitbit/oauth/";

            return Json("https://www.fitbit.com/oauth2/authorize?response_type=code&client_id=228PR8&redirect_uri=http%3A%2F%2Fmusgrosoft-health-api.azurewebsites.net%2Fapi%2Ffitbit%2Foauth%2F&scope=activity%20heartrate");
        }


    }
}