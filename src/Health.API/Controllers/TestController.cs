using Importer.GoogleSheets;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Repositories.Health.Models;


namespace HealthAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Test")]
    public class TestController : Controller
    {
        private readonly ISheetsService _sheetsService;

        public TestController(ISheetsService sheetsService)
        {
            _sheetsService = sheetsService;
        }

        [HttpPost]
        [Route("exercise")]
        //[EnableCors("CorsPolicy")]
        [EnableCors]
        //public async Task<IActionResult> Migrate()
        public IActionResult Test([FromBody]Exercise exercise)
        {
            _sheetsService.InsertExercises(exercise);

            return Ok();
        }
    }
}