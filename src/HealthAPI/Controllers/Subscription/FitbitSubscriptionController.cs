using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Fitbit;
using Utils;

namespace HealthAPI.Controllers.Subscription
{
    [Produces("application/json")]
    [Route("api/Fitbit/Notification")]
    public class FitbitSubscriptionController : Controller
    {
        private readonly ILogger _logger;
        private readonly IConfig _config;
        private readonly IFitbitClient _fitbitClient;

        public FitbitSubscriptionController(ILogger logger, IConfig config, IFitbitClient fitbitClient)
        {
            _logger = logger;
            _config = config;
            _fitbitClient = fitbitClient;
        }

        [HttpPost]
        public IActionResult Notify([FromBody] List<Note> notifications)
        {
            _logger.Log("hello 234");
            _logger.Log(notifications.ToString());
            _logger.Log(JsonConvert.SerializeObject(notifications));
          

            return (NoContent());
        }


        [HttpGet]
        public IActionResult Verify(string verify)
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
        [Route("/Subscribe")]
        public IActionResult Subscribe(string verify)
        {
            _fitbitClient.Subscribe();
            return Ok();
        }

    }
}