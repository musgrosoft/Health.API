using Microsoft.AspNetCore.Mvc;
using Services.MyHealth;

namespace HealthAPI.Controllers.Data
{
    [Produces("application/json")]
    [Route("api/Weights")]
    public class WeightsController : Controller
    {
        private readonly IHealthService _healthService;

        public WeightsController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_healthService.GetAllWeights());
        }


    }
}