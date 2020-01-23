using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace Health.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly HealthContext _healthContext;

        public DatabaseController(HealthContext healthContext)
        {
            _healthContext = healthContext;
        }

        [HttpGet]
        public IActionResult Drop()
        {
            var result = _healthContext.Database.EnsureDeleted();

            return Ok(result);
        }
    }
}