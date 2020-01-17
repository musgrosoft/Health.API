using System;
using System.Threading.Tasks;
using Fitbit;
using Microsoft.AspNetCore.Mvc;

namespace Health.API.Controllers
{
    [Route("api/canary")]
    [ApiController]
    public class CanaryController : ControllerBase
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
        public async Task<IActionResult> Fitbit()
        {
            var sleeps = await _fitbitService.GetSleepSummaries(DateTime.Now.AddDays(-10), DateTime.Now);

            return Ok(sleeps);
        }
    }
}