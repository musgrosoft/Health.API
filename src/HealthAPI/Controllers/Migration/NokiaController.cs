using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Migrators;
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
        private readonly INokiaAuthenticator _nokiaAuthenticator;
        private readonly INokiaClient _nokiaClient;


        public NokiaController(ILogger logger, INokiaMigrator nokiaMigrator, INokiaAuthenticator nokiaAuthenticator, INokiaClient nokiaClient)
        {
            _logger = logger;
            _nokiaMigrator = nokiaMigrator;
            _nokiaAuthenticator = nokiaAuthenticator;
            _nokiaClient = nokiaClient;
        }
        
        //todo post
       // [HttpGet]
        public async Task<IActionResult> Migrate()
        {
            //http://www.yourdomain.net/yourCustomApplication.php ?userid=123456&startdate=1260350649 &enddate=1260350650&appli=44

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

        [HttpGet]
        [Route("OAuth")]
        public async Task<IActionResult> OAuth([FromQuery]string code)
        {
            await _nokiaAuthenticator.SetTokens(code);
            return Ok("Helllo123");
        }
        

        [HttpGet]
        [Route("Subscribe")]
        public async Task<IActionResult> Subscribe()
        {
            await _nokiaClient.Subscribe();
            return Ok("Helllo234");
        }

        [HttpGet]
        [Route("ListSubscriptions")]
        public async Task<IActionResult> ListSubscriptions()
        {
            var subscriptions = await _nokiaClient.GetSubscriptions();

            return Ok(subscriptions);
        }

    }
}