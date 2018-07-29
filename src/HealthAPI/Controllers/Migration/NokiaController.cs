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

        private const int WeightKgMeasureTypeId = 1;
        private const int FatRatioPercentageMeasureTypeId = 6;
        private const int DiastolicBloodPressureMeasureTypeId = 9;
        private const int SystolicBloodPressureMeasureTypeId = 10;
        private const int SubscribeBloodPressureId = 4;


        public NokiaController(ILogger logger, INokiaMigrator nokiaMigrator, INokiaAuthenticator nokiaAuthenticator, INokiaClient nokiaClient)
        {
            _logger = logger;
            _nokiaMigrator = nokiaMigrator;
            _nokiaAuthenticator = nokiaAuthenticator;
            _nokiaClient = nokiaClient;
        }
        


        //todo post
       // [HttpGet]
        public async Task<IActionResult> Migrate([FromQuery]int? appli)
        {
            //http://www.yourdomain.net/yourCustomApplication.php ?userid=123456&startdate=1260350649 &enddate=1260350650&appli=44

            try
            {
                _logger.Log("NOKIA : starting nokia migrate");

                if (!appli.HasValue)
                {
                    _logger.Log("NOKIA : Migrating both Weights and Bloodpressures");
                    await _nokiaMigrator.MigrateWeights();
                    await _nokiaMigrator.MigrateBloodPressures();
                }
                else
                {
                    if (appli == WeightKgMeasureTypeId)
                    {
                        _logger.Log("NOKIA : Migrating just weights");
                        await _nokiaMigrator.MigrateWeights();
                    }

                    if (appli == SubscribeBloodPressureId)
                    {
                        _logger.Log("NOKIA : Migrating just blood pressures");
                        await _nokiaMigrator.MigrateBloodPressures();
                    }

                }
                
                _logger.Log("NOKIA : finishing nokia migrate");

                return Ok();

            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return NotFound(ex.ToString());
            }

        }

        [Route("Notify/Weights")]
        public async Task<IActionResult> MigrateWeights()
        {
            //http://www.yourdomain.net/yourCustomApplication.php ?userid=123456&startdate=1260350649 &enddate=1260350650&appli=44

            _logger.Log("NOKIA : Migrating just weights");

            await _nokiaMigrator.MigrateWeights();

            _logger.Log("NOKIA : Finished Migrating just weights");

            return Ok();
        }

        [Route("Notify/BloodPressures")]
        public async Task<IActionResult> MigrateBloodPressures()
        {
            //http://www.yourdomain.net/yourCustomApplication.php ?userid=123456&startdate=1260350649 &enddate=1260350650&appli=44

            _logger.Log("NOKIA : Migrating just blood pressures");

            await _nokiaMigrator.MigrateBloodPressures();

            _logger.Log("NOKIA : Finished Migrating just blood pressures");

            return Ok();
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