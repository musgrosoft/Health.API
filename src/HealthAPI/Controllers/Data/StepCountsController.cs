using Microsoft.AspNetCore.Mvc;
using Services.Health;

namespace HealthAPI.Controllers.Data
{
    [Produces("application/json")]
    [Route("api/StepCounts")]
    public class StepCountsController : Controller
    {
        private readonly IHealthService _healthService;

        public StepCountsController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_healthService.GetAllStepCounts());
        }

        [HttpGet]
        [Route("GroupByWeek")]
        public IActionResult GetByWeek()
        {
            return Json(_healthService.GetAllStepCountsByWeek());
        }

        [HttpGet]
        [Route("GroupByMonth")]
        public IActionResult GetByMonth()
        {
            return Json(_healthService.GetAllStepCountsByMonth());
        }



    }
}