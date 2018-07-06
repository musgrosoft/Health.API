using Microsoft.AspNetCore.Mvc;
using Services.MyHealth;

namespace HealthAPI.Controllers.Data
{
    [Produces("application/json")]
    [Route("api/AlcoholIntakes")]
    public class AlcoholIntakesController : Controller
    {
        private readonly IHealthService _healthService;

        public AlcoholIntakesController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_healthService.GetAllAlcoholIntakes());
        }

        [HttpGet]
        [Route("GroupByWeek")]
        public IActionResult GetByWeek()
        {
            return Json(_healthService.GetAllAlcoholIntakesByWeek());
        }

        [HttpGet]
        [Route("GroupByMonth")]
        public IActionResult GetByMonth()
        {
            return Json(_healthService.GetAllAlcoholIntakesByMonth());
        }
    }
}