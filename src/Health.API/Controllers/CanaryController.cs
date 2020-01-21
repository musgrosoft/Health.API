using System;
using System.Linq;
using System.Threading.Tasks;
using Fitbit;
using GoogleSheets;
using Microsoft.AspNetCore.Mvc;
using Repositories.Health;
using Withings;

namespace Health.API.Controllers
{
    [Route("api/canary")]
    [ApiController]
    public class CanaryController : ControllerBase
    {
        private readonly IFitbitService _fitbitService;
        private readonly IWithingsService _withingsService;
        private readonly ISheetsService _sheetsService;
        private readonly IHealthRepository _healthRepository;

        public CanaryController(IFitbitService fitbitService, IWithingsService withingsService, ISheetsService sheetsService, IHealthRepository healthRepository)
        {
            _fitbitService = fitbitService;
            _withingsService = withingsService;
            _sheetsService = sheetsService;
            _healthRepository = healthRepository;
        }

        [HttpGet]
        //[Route("Index")]
        public async Task<IActionResult> Index()
        {
            var sleeps = await _fitbitService.GetSleepSummaries(DateTime.Now.AddDays(-30), DateTime.Now);
            var restingHeartRates = await _fitbitService.GetRestingHeartRates(DateTime.Now.AddDays(-30), DateTime.Now);

            var weights = await _withingsService.GetWeights(DateTime.Now.AddDays(-30));
            var bloodPressures = await _withingsService.GetBloodPressures(DateTime.Now.AddDays(-30));

            var targets = await _sheetsService.GetTargets();
            var exercises = await _sheetsService.GetExercises(DateTime.Now.AddDays(-30));
            var drinks = await _sheetsService.GetDrinks(DateTime.Now.AddDays(-30));

            var dbWeightDate = _healthRepository.GetLatestWeightDate();

            var resp = new CanaryResponse
            {
                FitbitSleepSummaries = sleeps.Any(),
                FitbitRestingHeartRates = restingHeartRates.Any(),
                WithingsWeights = weights.Any(),
                WithingsBloodPressures = bloodPressures.Any(),
                GoogleSheetsTargets = targets.Any(),
                GoogleSheetsExercises = exercises.Any(),
                GoogleSheetsDrinks = drinks.Any(),
                DatabaseQuery = dbWeightDate.HasValue
            };

            return Ok(resp);
        }
    }

    public class CanaryResponse
    {
        public bool FitbitSleepSummaries { get; set; }
        public bool FitbitRestingHeartRates { get; set; }
        public bool WithingsWeights { get; set; }
        public bool WithingsBloodPressures { get; set; }
        public bool GoogleSheetsTargets { get; set; }
        public bool GoogleSheetsExercises { get; set; }
        public bool GoogleSheetsDrinks { get; set; }
        public bool DatabaseQuery { get; set; }
    }
}