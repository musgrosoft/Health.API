using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fitbit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Health.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanaryController : ControllerBase
    {
        private readonly IFitbitService _fitbitService;

        public CanaryController(IFitbitService fitbitService)
        {
            _fitbitService = fitbitService;
        }

        [HttpGet]
        //[Route("Notification")]
        public IActionResult Index()
        {
            return Ok("Hello Tim");
        }

        [HttpGet]
        //[Route("Notification")]
        public IActionResult Fitbit()
        {
            var sleeps = _fitbitService.GetSleepSummaries(DateTime.Now.AddDays(-10), DateTime.Now);

            return Ok(sleeps);
        }
    }
}