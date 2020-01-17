using System;
using Fitbit;
using Microsoft.AspNetCore.Mvc;

namespace Health.API.Controllers
{
    [Route("api/canary")]
    [ApiController]
    public class CanaryController : Controller
    {
        private readonly IFitbitService _fitbitService;

        public CanaryController(IFitbitService fitbitService)
        {
            _fitbitService = fitbitService;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            return Ok("Hello Tim");
        }

        [HttpGet]
        [Route("Fitbit")]
        public IActionResult Fitbit()
        {
            var sleeps = _fitbitService.GetSleepSummaries(DateTime.Now.AddDays(-10), DateTime.Now);

            return Ok(sleeps);
        }
    }
}