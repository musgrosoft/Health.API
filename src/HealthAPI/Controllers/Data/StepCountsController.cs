using Microsoft.AspNetCore.Mvc;
using Services.MyHealth;

namespace HealthAPI.Controllers.Data
{
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
        public IActionResult GetByWeek()
        {
            return Json(_healthService.GetAllStepCountsByWeek());
        }

        [HttpGet]
        public IActionResult GetByMonth()
        {
            return Json(_healthService.GetAllStepCountsByMonth());
        }



    }
}