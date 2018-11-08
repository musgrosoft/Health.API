using System;
using System.Threading.Tasks;
using Fitbit.Services;
using Hangfire;
using HealthAPI.Hangfire;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace HealthAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Fitbit")]
    public class FitbitController : Controller
    {
        private readonly ILogger _logger;
        private readonly IConfig _config;
        private readonly IFitbitService _fitbitService;
       // private readonly IHangfireUtility _hangfireUtility;
        private readonly IHangfireWork _hangfireWork;

        private readonly IBackgroundJobClient _backgroundJobClient;

        public FitbitController(ILogger logger, IConfig config, IFitbitService fitbitService, IBackgroundJobClient backgroundJobClient, IHangfireWork hangfireWork)
        {
             
            _logger = logger;
            _config = config;
            _fitbitService = fitbitService;
            _backgroundJobClient = backgroundJobClient;
            //_hangfireUtility = hangfireUtility;
            _hangfireWork = hangfireWork;
        }

        [HttpPost]
        [Route("Notification")]
        public IActionResult Notify()
        {
            _logger.LogMessageAsync("Fitbit Notification : MIGRATING ALL FITBIT DATA");

            //_hangfireUtility.Enqueue(() => _hangfireWork.MigrateAllFitbitData());
            _backgroundJobClient.Enqueue(() => _hangfireWork.MigrateAllFitbitData());

            return (NoContent());
        }

        [HttpGet]
        [Route("Notification")]
        public IActionResult Verify([FromQuery]string verify)
        {
            if (verify == _config.FitbitVerificationCode)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("Subscribe")]
        public async Task<IActionResult> Subscribe()
        {
            await _fitbitService.Subscribe();
            return Ok();
        }

        [HttpGet]
        [Route("OAuth")]
        public async Task<IActionResult> OAuth([FromQuery]string code)
        {   
            await _fitbitService.SetTokens(code);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(typeof(String), 200)]
        [Route("OAuthUrl")]
        public IActionResult OAuthUrl()
        {
            //var redirectUrl = "http://musgrosoft-health-api.azurewebsites.net/api/fitbit/oauth/";
            var redirectUrl = _config.FitbitOAuthRedirectUrl;
            var urlEncodedRedirectUrl = Uri.EscapeDataString(redirectUrl);


            //return Json("https://www.fitbit.com/oauth2/authorize?response_type=code&client_id=228PR8&redirect_uri=http%3A%2F%2Fmusgrosoft-health-api.azurewebsites.net%2Fapi%2Ffitbit%2Foauth%2F&scope=activity%20heartrate");
            return Json($"https://www.fitbit.com/oauth2/authorize?response_type=code&client_id=228PR8&redirect_uri={urlEncodedRedirectUrl}&scope=activity%20heartrate");
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(String), 200)]
        //[Route("Runs")]
        //public async Task<IActionResult> Runs()
        //{
        //    var runs = await _fitbitService.GetRuns(new DateTime(2018, 8, 1), new DateTime(2018, 10, 11));
        //    return Json(runs);
        //}
    }
}