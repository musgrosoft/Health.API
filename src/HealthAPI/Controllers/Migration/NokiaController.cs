using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthAPI;
using HealthAPI.Models;
using Migrators;
using Services.MyHealth;
using Services.Nokia;
using Utils;

namespace HealthAPI.Controllers
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
        [HttpPost]
        public async Task<IActionResult> Migrate()
        {

            try
            {
                //var logger = context.Logger;
                var logger = new Logger();

                //logger.Log("STARTING NOKIA MIGRATOR");
                var healthService = HealthServiceFactory.Build( logger);

                var nokiaMigrator = new NokiaMigrator(healthService, logger, new NokiaClient());

                await nokiaMigrator.MigrateWeights();
                await nokiaMigrator.MigrateBloodPressures();


                return Ok();
                //return new APIGatewayProxyResponse
                //{
                //    StatusCode = (int)HttpStatusCode.OK
                //};

            }
            catch (Exception ex)
            {
                //LambdaLogger.Log(ex.ToString());

                //return new APIGatewayProxyResponse
                //{
                //    StatusCode = (int)HttpStatusCode.InternalServerError,
                //    Body = $"Uh oh, {ex.ToString()}"
                //};

            }
            return Ok();
        }

       
    }
}