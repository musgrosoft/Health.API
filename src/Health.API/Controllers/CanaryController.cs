﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public CanaryController(IHealthService healthService, ITokenService tokenService)
        {
            _healthService = healthService;
            _tokenService = tokenService;
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
                LatestRestingHeartRates = _healthService.GetLatestRestingHeartRates(20),
                LatestWeights = _healthService.GetLatestWeights(20),
                LatestBloodPressures = _healthService.GetLatestBloodPressures(20),
                LatestDrinks = _healthService.GetLatestDrinks(20),
                LatestExercises = _healthService.GetLatestExercises(20)
            });

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