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

        public NokiaController(HealthContext context)
        {
            _context = context;
        }

        // POST: api/AlcoholIntakes1
        [HttpGet]
     //   [Route("api/Nokia/Migrate")]
        public async Task<IActionResult> Migrate()
        {
            var logger = new Logger();
            try
            {
                //var logger = context.Logger;

                logger.Log("NOKIA : starting nokia migrate");

                //logger.Log("STARTING NOKIA MIGRATOR");
                var healthService = HealthServiceFactory.Build( logger, _context);

                var nokiaMigrator = new NokiaMigrator(healthService, logger, new NokiaClient(new HttpClient()));

                await nokiaMigrator.MigrateWeights();
                await nokiaMigrator.MigrateBloodPressures();

                healthService.UpsertAlcoholIntakes();

                logger.Log("NOKIA : finishing nokia migrate");

                return Ok();
                //return new APIGatewayProxyResponse
                //{
                //    StatusCode = (int)HttpStatusCode.OK
                //};

            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return NotFound(ex.ToString());
                //LambdaLogger.Log(ex.ToString());

                //return new APIGatewayProxyResponse
                //{
                //    StatusCode = (int)HttpStatusCode.InternalServerError,
                //    Body = $"Uh oh, {ex.ToString()}"
                //};

            }
          //  return Ok();
        }

       
    }
}