using Google;
using Microsoft.AspNetCore.Mvc;
using Services.GoogleSheets;
using Utils;

namespace HealthAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Google")]
    public class GoogleSheetsController : Controller
    {
        private readonly ILogger _logger;
        
        private readonly IGoogleImporter _googleImporter;

        public GoogleSheetsController(ILogger logger, IGoogleImporter googleImporter)
        {
            _logger = logger;
            _googleImporter = googleImporter;
        }


        [HttpGet]
        [Route("Notify/AlcoholIntakes")]
        //public async Task<IActionResult> Migrate()
        public IActionResult MigrateUnits()
        {
            _logger.LogMessageAsync("GOOGLE SHEETS : Migrate Units");

            _googleImporter.MigrateAlcoholIntakes();

            return Ok();
        }

        [HttpGet]
        [Route("Notify/Exercises")]
        //public async Task<IActionResult> Migrate()
        public IActionResult ImportExercises()
        {
            _logger.LogMessageAsync("GOOGLE SHEETS : Import Exercises");

            _googleImporter.ImportExercises();

            return Ok();
        }

    }

}