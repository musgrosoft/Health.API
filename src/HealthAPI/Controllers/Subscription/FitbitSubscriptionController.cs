using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Notify([FromBody] string content)
        {
            _logger.Log("hello 234");
            _logger.Log(content);
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