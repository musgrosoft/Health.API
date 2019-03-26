using Google;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebSockets.Internal;
using Repositories.Health.Models;
using Services.GoogleSheets;


namespace HealthAPI.Controllers
{
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    [Route("api/Test")]
    public class TestController : Controller
    {
        private readonly IGoogleImporter _googleImporter;

        public TestController(IGoogleImporter googleImporter)
        {
            _googleImporter = googleImporter;
        }

        [HttpPost]
        [Route("exercise")]
        [EnableCors("CorsPolicy")]
        //public async Task<IActionResult> Migrate()
        public IActionResult Test([FromBody]Exercise exercise)
        {
            _googleImporter.InsertExercises(exercise);

            return Ok();
        }
    }
}