using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Repositories.Health.Models;
using Services.Health;
using Services.OAuth;

namespace Health.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Canary")]
    [ApiController]
    public class CanaryController : ControllerBase
    {
        private readonly IHealthService _healthService;
        private readonly ITokenService _tokenService;
        private readonly IMessageMaker _messageMaker;

        public CanaryController(IHealthService healthService, ITokenService tokenService, IMessageMaker messageMaker)
        {
            _healthService = healthService;
            _tokenService = tokenService;
            _messageMaker = messageMaker;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var fitbitRefreshToken = await _tokenService.GetFitbitRefreshToken();
            var withingsRefreshToken = await _tokenService.GetWithingsRefreshToken();
            

            var messages = new List<string>();

            messages.Add(string.IsNullOrWhiteSpace(fitbitRefreshToken) ? "XXX Fitbit Refresh Token is empty" : "Fitbit Refresh Token is populated");
            messages.Add(string.IsNullOrWhiteSpace(withingsRefreshToken) ? "XXX Withings Refresh Token is empty" : "Withings Refresh Token is populated");

            return Ok(new CanaryData
            {
                Messages = messages,
//                LatestRestingHeartRates = _healthService.GetLatestRestingHeartRates(20),
//                LatestWeights = _healthService.GetLatestWeights(20),
//                LatestBloodPressures = _healthService.GetLatestBloodPressures(20),
//                LatestDrinks = _healthService.GetLatestDrinks(20),
//                LatestExercises = _healthService.GetLatestExercises(20)

                LatestRestingHeartRates = _healthService.GetLatestRestingHeartRates(20000),
                LatestWeights = _healthService.GetLatestWeights(20000),
                LatestBloodPressures = _healthService.GetLatestBloodPressures(20000),
                LatestDrinks = _healthService.GetLatestDrinks(20000),
                LatestExercises = _healthService.GetLatestExercises(20000)

            });

        }

        [HttpGet]
        [Route("Messages")]
        public async Task<IActionResult> Messages()
        {

            
            
            var errors = await _messageMaker.GetTechnicalErrorMessages();
            var oldData = _messageMaker.GetOldDataMessages();
            var targetMessages = _messageMaker.GetTargetMessages();
            var missedTargets = targetMessages.MissedTargets;
            var hitTargets = targetMessages.HitTargets;

            var messages = new List<string>();

            messages.AddRange(errors);
            messages.AddRange(oldData);
            messages.AddRange(missedTargets);
            messages.AddRange(hitTargets);


            return Ok(messages);

        }

        private class CanaryData
        {
            public List<string> Messages { get; set; }
            public List<RestingHeartRate> LatestRestingHeartRates { get; set; }
            public List<Weight> LatestWeights { get; set; }
            public List<BloodPressure> LatestBloodPressures { get; set; }
            public List<Drink> LatestDrinks { get; set; }
            public List<Exercise> LatestExercises { get; set; }
        }
    }
}