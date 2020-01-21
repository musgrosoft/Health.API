using System;
using System.Linq;
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
        public async Task<IActionResult> Index()
        {
            var sleeps = await _fitbitService.GetSleepSummaries(DateTime.Now.AddDays(-10), DateTime.Now);

            var resp = new CanaryResponse
            {
                FitbitSleepSummaries = sleeps.Any()
            };

            return Ok(resp);
        }
    }

    public class CanaryResponse
    {
        public bool FitbitSleepSummaries { get; set; }
    }
}