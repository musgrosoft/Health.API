using Microsoft.AspNetCore.Mvc;
using Services.Health;

namespace HealthAPI.Controllers.Data
{
    [Produces("application/json")]
    [Route("api/Runs")]
    public class RunsController : Controller
    {
        private readonly IHealthService _healthService;

        public RunsController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_healthService.GetAllRuns());
        }


    }
}