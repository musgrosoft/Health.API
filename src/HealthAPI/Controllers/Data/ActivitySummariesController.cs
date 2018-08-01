using Microsoft.AspNetCore.Mvc;
using Services.Health;

namespace HealthAPI.Controllers.Data
{
    [Produces("application/json")]
    [Route("api/ActivitySummaries")]
    public class ActivitySummariesController : Controller
    {
        private readonly IHealthService _healthService;

        public ActivitySummariesController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_healthService.GetAllActivitySummaries());
        }

        [HttpGet]
        [Route("GroupByWeek")]
        public IActionResult GetByWeek()
        {
            return Json(_healthService.GetAllActivitySummariesByWeek());
        }

        [HttpGet]
        [Route("GroupByMonth")]
        public IActionResult GetByMonth()
        {
            return Json(_healthService.GetAllActivitySummariesByMonth());
        }
    }
}
