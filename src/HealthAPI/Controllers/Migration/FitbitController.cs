using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Migrators;
using Repositories;
using Repositories.OAuth;
using Services.Fitbit;
using Services.MyHealth;
using Services.Nokia;
using Services.OAuth;
using Utils;
using Services;

namespace HealthAPI.Controllers.Migration
{
    [Produces("application/json")]
    [Route("api/Fitbit")]
    public class FitbitController : Controller
    {
        private readonly ILogger _logger;
        private readonly IOAuthService _oAuthService;
        private readonly IFitbitMigrator _fitbitMigrator;

        public FitbitController(
            ILogger logger, 
            IOAuthService oAuthService, 
            IFitbitMigrator fitbitMigrator)
        {
            
            _logger = logger;
            _oAuthService = oAuthService;
            _fitbitMigrator = fitbitMigrator;
        }

        //todo post
        [HttpGet]
        public async Task<IActionResult> Migrate()
        {   
            try
            {
                _logger.Log("FITBIT : starting fitbit migrate");
                
                var v = await _oAuthService.GetFitbitRefreshToken();

                //monthly gets
                await _fitbitMigrator.MigrateRestingHeartRates();
                await _fitbitMigrator.MigrateHeartSummaries();
                //daily gets
                await _fitbitMigrator.MigrateStepCounts();
                await _fitbitMigrator.MigrateActivitySummaries();
              
                _logger.Log("FITBIT : finishing fitbit migrate");

                return Ok();

            }
            catch (Exception ex)
            {
                
                _logger.Error(ex);
                //todo return error
                return NotFound(ex.ToString());
                

            }
        }

       
    }
}