using Microsoft.AspNetCore.Mvc;
using Services.MyHealth;

namespace HealthAPI.Controllers.Data
{
    [Produces("application/json")]
    [Route("api/HeartRateSummaries")]
    public class HeartRateSummariesController : Controller
    {
        private readonly IHealthService _healthService;

        public HeartRateSummariesController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_healthService.GetAllHeartRateSummaries());
        }

        [HttpGet]
        [Route("GroupByWeek")]
        public IActionResult GetByWeek()
        {
            return Json(_healthService.GetAllHeartRateSummariesByWeek());
        }

        [HttpGet]
        [Route("GroupByMonth")]
        public IActionResult GetByMonth()
        {
            return Json(_healthService.GetAllHeartRateSummariesByMonth());

        }
    }
}