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
        private readonly IGoogleClient _googleClient;

        public TestController(IGoogleClient googleClient)
        {
            _googleClient = googleClient;
        }

        [HttpPost]
        [Route("exercise")]
        [EnableCors("CorsPolicy")]
        //public async Task<IActionResult> Migrate()
        public IActionResult Test([FromBody]Exercise exercise)
        {
            _googleClient.InsertExercises(exercise);

            return Ok();
        }
    }
}