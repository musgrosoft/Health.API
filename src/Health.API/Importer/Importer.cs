using System;
using System.Linq;
using System.Threading.Tasks;
using Fitbit;
using GoogleSheets;
using Services.Health;
using Utils;
using Withings;

namespace HealthAPI.Importer
{
    public class Importer : IImporter
    {

        private ILogger _logger;
        private IHealthService _healthService;
        private ICalendar _calendar;
        private IFitbitService _fitbitService;

        private const int SEARCH_DAYS_PREVIOUS = 10;

        private DateTime MIN_FITBIT_DATE = new DateTime(2017, 5, 1);
        private DateTime MIN_FITBIT_SLEEP_DATE = new DateTime(2019, 7, 1);

        private readonly IWithingsService _withingsService;
        private readonly ISheetsService _sheetsService;


        private readonly DateTime MIN_WEIGHT_DATE = new DateTime(2012, 1, 1);
        private readonly DateTime MIN_BLOOD_PRESSURE_DATE = new DateTime(2012, 1, 1);


        private readonly DateTime MIN_DRINK_DATE = new DateTime(2016, 1, 1);
        private readonly DateTime MIN_EXERCISEE_DATE = new DateTime(2017, 5, 3);

        public Importer(ILogger logger, IHealthService healthService, ICalendar calendar, IFitbitService fitbitService, IWithingsService withingsService, ISheetsService sheetsService)
        {
            _logger = logger;
            _healthService = healthService;
            _calendar = calendar;
            _fitbitService = fitbitService;
            _withingsService = withingsService;
            _sheetsService = sheetsService;
        }


        public async Task ImportFitbitRestingHeartRates()
        {
            var latestRestingHeartRateDate = _healthService.GetLatestRestingHeartRateDate(MIN_FITBIT_DATE);
            var fromDate = latestRestingHeartRateDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            await _logger.LogMessageAsync($"RESTING HEART RATES : Latest record has a date of : {latestRestingHeartRateDate:dd-MMM-yyyy HH:mm:ss (ddd)}, retrieving records from {SEARCH_DAYS_PREVIOUS} days previous : {fromDate:dd-MMM-yyyy HH:mm:ss(ddd)}");

            var restingHeartRates = await _fitbitService.GetRestingHeartRates(fromDate, _calendar.Now());

            await _logger.LogMessageAsync($"RESTING HEART RATES : found {restingHeartRates.Count()} records");

            //todo figure out why this has duplicate entries in it.
            restingHeartRates = restingHeartRates
                .GroupBy(x => x.CreatedDate)
                .Select(group => group.First());

            await _logger.LogMessageAsync($"RESTING HEART RATES : distinct count is {restingHeartRates.Count()} records");

            if (restingHeartRates.Any())
            {
                await _logger.LogMessageAsync($"RESTING HEART RATES : First at {restingHeartRates.Min(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)} , last at {restingHeartRates.Max(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)}.");
            }



            await _healthService.UpsertAsync(restingHeartRates);

            await _logger.LogMessageAsync($"RESTING HEART RATES : Finished Importing.");

        }

        public async Task ImportFitbitSleepSummaries()
        {
            var latestFitbitSleepSummaryDate = _healthService.GetLatestSleepSummaryDate(MIN_FITBIT_SLEEP_DATE);
            var fromDate = latestFitbitSleepSummaryDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            await _logger.LogMessageAsync($"SLEEP SUMMARIES : Latest record has a date of : {latestFitbitSleepSummaryDate:dd-MMM-yyyy HH:mm:ss (ddd)}, retrieving records from {SEARCH_DAYS_PREVIOUS} days previous : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}.");

            var sleepSummaries = await _fitbitService.GetSleepSummaries(fromDate, _calendar.Now());

            await _logger.LogMessageAsync($"SLEEP SUMMARIES : found {sleepSummaries.Count()} records");

            if (sleepSummaries.Any())
            {
                await _logger.LogMessageAsync($"SLEEP SUMMARIES : First at {sleepSummaries.Min(x => x.DateOfSleep):dd-MMM-yyyy HH:mm:ss (ddd)} , last at {sleepSummaries.Max(x => x.DateOfSleep):dd-MMM-yyyy HH:mm:ss (ddd)}.");
            }

            await _healthService.UpsertAsync(sleepSummaries);

            await _logger.LogMessageAsync($"SLEEP SUMMARIES : Finished Importing.");
        }

        public async Task ImportWithingsWeights()
        {
            await _logger.LogMessageAsync("WEIGHTS : NOTIFICATION from Withings");

            var latestWeightDate = _healthService.GetLatestWeightDate(MIN_WEIGHT_DATE);
            var fromDate = latestWeightDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            await _logger.LogMessageAsync($"WEIGHTS : Latest record has a date of : {latestWeightDate:dd-MMM-yyyy HH:mm:ss (ddd)}, will retrieve from {SEARCH_DAYS_PREVIOUS} days previous to this date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}.");

            var weights = (await _withingsService.GetWeights(fromDate)).ToList();

            await _logger.LogMessageAsync($"WEIGHTS : Found {weights.Count()} records.");

            if (weights.Any())
            {
                await _logger.LogMessageAsync($"WEIGHTS : First at {weights.Min(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)} , last at {weights.Max(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)}.");
            }

            await _healthService.UpsertAsync(weights);

            await _logger.LogMessageAsync("WEIGHTS: Finished Importing.");

        }

        public async Task ImportWithingsBloodPressures()
        {
            await _logger.LogMessageAsync("BLOOD PRESSURES : NOTIFICATION (from Withings)");

            var latestBloodPressureDate = _healthService.GetLatestBloodPressureDate(MIN_BLOOD_PRESSURE_DATE);
            var fromDate = latestBloodPressureDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            await _logger.LogMessageAsync($"BLOOD PRESSURES : Latest record has a date of : {latestBloodPressureDate:dd-MMM-yyyy HH:mm:ss (ddd)}, will retrieve from {SEARCH_DAYS_PREVIOUS} days previous to this date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}.");

            var bloodPressures = await _withingsService.GetBloodPressures(fromDate);

            await _logger.LogMessageAsync($"BLOOD PRESSURES : Found {bloodPressures.Count()} records.");

            if (bloodPressures.Any())
            {
                await _logger.LogMessageAsync($"BLOOD PRESSURES : First at {bloodPressures.Min(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)} , last at {bloodPressures.Max(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)}.");
            }

            await _healthService.UpsertAsync(bloodPressures);

            await _logger.LogMessageAsync("BLOOD PRESSURES: Finished Importing.");

        }


        public async Task ImportGoogleSheetsDrinks()
        {
            await _logger.LogMessageAsync("DRINKS : Notification (from Google Sheets)");

            var latestDrinkDate = _healthService.GetLatestDrinkDate(MIN_DRINK_DATE);
            var fromDate = latestDrinkDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            await _logger.LogMessageAsync($"DRINKS : Latest record has a date of : {latestDrinkDate:dd-MMM-yyyy HH:mm:ss (ddd)}, will retrieve from {SEARCH_DAYS_PREVIOUS} days previous to this date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}.");

            var drinks = await _sheetsService.GetDrinks(fromDate);

            await _logger.LogMessageAsync($"DRINKS : Found {drinks.Count()} records.");

            if (drinks.Any())
            {
                await _logger.LogMessageAsync($"DRINKS : First at {drinks.Min(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)} , last at {drinks.Max(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)}.");
            }

            await _healthService.UpsertAsync(drinks);

            await _logger.LogMessageAsync("DRINKS: Finished Importing.");

        }

        public async Task ImportGoogleSheetsExercises()
        {
            await _logger.LogMessageAsync("EXERCISES : Notification (from Google Sheets).");

            var latestExerciseDate = _healthService.GetLatestExerciseDate(MIN_EXERCISEE_DATE);
            var fromDate = latestExerciseDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            await _logger.LogMessageAsync($"EXERCISES : Latest record has a date of : {latestExerciseDate:dd-MMM-yyyy HH:mm:ss (ddd)}, will retrieve from {SEARCH_DAYS_PREVIOUS} days previous to this date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}.");

            var exercises = await _sheetsService.GetExercises(fromDate);

            await _logger.LogMessageAsync($"EXERCISES : Found {exercises.Count()} records.");

            if (exercises.Any())
            {
                await _logger.LogMessageAsync($"EXERCISES : First at {exercises.Min(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)} , last at {exercises.Max(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)}.");
            }

            await _healthService.UpsertAsync(exercises);


            await _logger.LogMessageAsync("EXERCISES: Finished Importing.");

        }


        public async Task ImportGoogleSheetsTargets()
        {
            var targets = await _sheetsService.GetTargets();

            await _healthService.UpsertAsync(targets);

        }

        public async Task ImportGoogleSheetsGarminRestingHeartRates()
        {
            var garminRestingHeartRates = await _sheetsService.GetGarminRestingHeartRates();

            await _healthService.UpsertAsync(garminRestingHeartRates);

        }

        public async Task ImportGoogleSheetsGarminIntensityMinutes()
        {
            var garminIntensityMinutes = await _sheetsService.GetGarminIntensityMinutes();

            await _healthService.UpsertAsync(garminIntensityMinutes);

        }
    }
}