using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Health;
using Services.OAuth;

namespace Health.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanaryController : ControllerBase
    {
        private readonly IHealthService _healthService;
        private readonly ITokenService _tokenService;

        public CanaryController(IHealthService healthService, ITokenService tokenService)
        {
            _healthService = healthService;
            _tokenService = tokenService;
        }

        public async Task<IActionResult> Index()
        {
            var fitbitRefreshToken = await _tokenService.GetFitbitRefreshToken();
            var withingsRefreshToken = await _tokenService.GetWithingsRefreshToken();

            var messages = new List<string>();

            messages.Add(string.IsNullOrWhiteSpace(fitbitRefreshToken) ? "XXX Fitbit Refresh Token is empty" : "Fitbit Refresh Token is populated");
            messages.Add(string.IsNullOrWhiteSpace(withingsRefreshToken) ? "XXX Withings Refresh Token is empty" : "Withings Refresh Token is populated");

            return Ok(messages);

        }
    }
}