using Microsoft.AspNetCore.Mvc;
using Services.Health;

namespace HealthAPI.Controllers.Data
{
    [Produces("application/json")]
    [Route("api/Ergos")]
    public class ErgosController : Controller
    {
        private readonly IHealthService _healthService;

        public ErgosController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_healthService.GetAllErgos());
        }


    }
}