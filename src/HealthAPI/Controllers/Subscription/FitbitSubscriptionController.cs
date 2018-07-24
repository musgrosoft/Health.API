using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utils;

namespace HealthAPI.Controllers.Subscription
{
    [Produces("application/json")]
    [Route("api/Fitbit/Notification")]
    public class FitbitSubscriptionController : Controller
    {
        private readonly ILogger _logger;

        public FitbitSubscriptionController(ILogger logger)
        {
            _logger = logger;
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
            if (verify == "123")
            {
                return (NoContent());
            }
            else
            {
                return (NotFound());
            }
        }



    }
}