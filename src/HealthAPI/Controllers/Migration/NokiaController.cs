using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Migrators;
using Utils;

namespace HealthAPI.Controllers.Migration
{
    [Produces("application/json")]
    [Route("api/Nokia")]
    public class NokiaController : Controller
    {
        private readonly ILogger _logger;
        private readonly INokiaMigrator _nokiaMigrator;

        public NokiaController(ILogger logger, INokiaMigrator nokiaMigrator)
        {
            this._logger = logger;
            this._nokiaMigrator = nokiaMigrator;
        }
        
        //todo post
        [HttpGet]
        public async Task<IActionResult> Migrate()
        {
            try
            {
                _logger.Log("NOKIA : starting nokia migrate");
                
                await _nokiaMigrator.MigrateWeights();
                await _nokiaMigrator.MigrateBloodPressures();


                _logger.Log("NOKIA : finishing nokia migrate");

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                //todo error
                return NotFound(ex.ToString());
                

            }

        }


        //https://account.health.nokia.com/oauth2_user/authorize2?response_type=code&client_id=09d4e17f36ee237455246942602624feaad12ac51598859bc79ddbd821147942&state=hello&scope=user.metrics&redirect_uri=http%3A%2F%2Fmusgrosoft-health-api.azurewebsites.net%2Fapi%2Fnokia%2Foauth
//        https://account.health.nokia.com/oauth2/token?grant_type=authorization_code&client_id=xxxxxxxxxxxxxxxxxx&client_secret=xxxxxxxxxxxxx&code=xxxxxxxxxxxxxxx&redirect_uri=xxxxxxxxxxxxxxx
        [HttpGet]
        [Route("OAuth")]
        public async Task<IActionResult> OAuth()
        {
            return Ok("Helllo");
        }
        //[HttpGet]
            //public async Task<IActionResult> SubscribeWeight()
            //{

            //}

        }
}