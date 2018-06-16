using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Migrators;
using Repositories;
using Services.MyHealth;
using Services.Nokia;
using Utils;

namespace HealthAPI.Controllers.Migration
{
    [Produces("application/json")]
    [Route("api/Nokia")]
    public class NokiaController : Controller
    {
        private readonly HealthContext _context;
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

       
    }
}