using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;

namespace HealthAPI.Controllers.OAuth
{
    [Produces("application/json")]
    [Route("api/OAuthUrls")]
    public class OAuthUrlsController : Controller
    {

        [HttpGet]
        [ProducesResponseType(typeof(IList<Weight>), 200)]
        public IActionResult Get()
        {
            var redirectUrl = "http://musgrosoft-health-api.azurewebsites.net/api/fitbit/oauth/";

            return Json(new List<string>
            {
                "https://www.fitbit.com/oauth2/authorize?response_type=code&client_id=228PR8&redirect_uri=http%3A%2F%2Fmusgrosoft-health-api.azurewebsites.net%2Fapi%2Ffitbit%2Foauth%2F&scope=activity%20heartrate",
                "https://account.health.nokia.com/oauth2_user/authorize2?response_type=code&redirect_uri=http://musgrosoft-health-api.azurewebsites.net/api/nokia/oauth/&client_id=09d4e17f36ee237455246942602624feaad12ac51598859bc79ddbd821147942&scope=user.info,user.metrics,user.activity&state=768uyFys"


            });
        }
    }

}
