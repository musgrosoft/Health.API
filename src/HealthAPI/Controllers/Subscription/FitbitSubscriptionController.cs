using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers.Subscription
{
    [Produces("application/json")]
    [Route("api/FitbitSubscription")]
    public class FitbitSubscriptionController : Controller
    {
        [HttpPost]
        public IActionResult Notify()
        {
            return (NoContent());
        }
    }
}