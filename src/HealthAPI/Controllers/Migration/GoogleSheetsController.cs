using System;
using Microsoft.AspNetCore.Mvc;
using Migrators;
using Utils;

namespace HealthAPI.Controllers.Migration
{
    [Produces("application/json")]
    [Route("api/Google")]
    public class GoogleSheetsController : Controller
    {
        private readonly ILogger _logger;
        
        private readonly IGoogleMigrator _googleMigrator;

        public GoogleSheetsController(ILogger logger, IGoogleMigrator googleMigrator)
        {
            _logger = logger;
            _googleMigrator = googleMigrator;
        }

        [HttpGet]
        //public async Task<IActionResult> Migrate()
        public IActionResult Migrate()
        {

            try
            {
                //var logger = context.Logger;
                _logger.Log("GOOGLE SHEETS : starting google migrate");

                _googleMigrator.MigrateRuns();
                _googleMigrator.MigrateAlcoholIntakes();

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