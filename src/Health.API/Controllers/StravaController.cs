using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace Health.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Strava")]
    public class StravaController : Controller
    {

        private readonly ILogger _logger;
        private readonly IConfig _config;
        private readonly IStravaService _stravaService;

        public StravaController(ILogger logger, IConfig config, IStravaService stravaService)
        {

            _logger = logger;
            _config = config;
            _stravaService = stravaService;
        }


        [HttpGet]
        [Route("OAuth")]
        public async Task<IActionResult> OAuth([FromQuery]string code)
        {
            await _stravaService.SetTokens(code);
            return Ok();
        }
    }

    public interface IStravaService
    {
        Task SetTokens(string code);
    }

    public class StravaService : IStravaService
    {
        public Task SetTokens(string code)
        {
            throw new System.NotImplementedException();
        }
    }


}