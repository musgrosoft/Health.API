using Microsoft.AspNetCore.Mvc;
using Services.Health;

namespace HealthAPI.Controllers.Data
{
    [Produces("application/json")]
    [Route("api/BloodPressures")]

    public class BloodPressuresController : Controller
    {
        private readonly IHealthService _healthService;

        public BloodPressuresController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_healthService.GetAllBloodPressures());
        }
    }
}