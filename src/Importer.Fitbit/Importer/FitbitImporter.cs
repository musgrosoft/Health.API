using System;
using System.Threading.Tasks;
using Services.Health;
using Utils;

namespace Importer.Fitbit.Importer
{
    public class FitbitImporter : IFitbitImporter
    {
        private readonly ILogger _logger;
        private IHealthService _healthService;
        private IFitbitService _fitbitService;
        private readonly ICalendar _calendar;

        private const int SEARCH_DAYS_PREVIOUS = 1;

        private DateTime MIN_FITBIT_DATE = new DateTime(2017, 5, 1);
        private DateTime MIN_FITBIT_SLEEP_DATE = new DateTime(2019, 7, 1);

        public FitbitImporter(IHealthService healthService, ILogger logger, IFitbitService fitbitService, ICalendar calendar)
        {
            _healthService = healthService;
            _logger = logger;
            _fitbitService = fitbitService;
            _calendar = calendar;
        }



        public async Task MigrateRestingHeartRates()
        {
            var latestRestingHeartRateDate = _healthService.GetLatestRestingHeartRateDate(MIN_FITBIT_DATE);
            await _logger.LogMessageAsync($"RESTING HEART RATE : Latest Resting Heart Rate record has a date of : {latestRestingHeartRateDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestRestingHeartRateDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            await _logger.LogMessageAsync($"RESTING HEART RATE : Retrieving Resting Heart Rate records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var restingHeartRates = await _fitbitService.GetRestingHeartRates(getDataFromDate, _calendar.Now());

            _healthService.UpsertRestingHeartRates(restingHeartRates);
        }

        public async Task MigrateSleeps()
        {
            var latestFitbitSleepDate = _healthService.GetLatestFitbitSleepDate(MIN_FITBIT_SLEEP_DATE);
            await _logger.LogMessageAsync($"SLEEP : Latest Sleep record has a date of : {latestFitbitSleepDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestFitbitSleepDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            await _logger.LogMessageAsync($"SLEEP : Retrieving Sleep records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var fitbitSleeps = await _fitbitService.GetSleeps(getDataFromDate, _calendar.Now());

            _healthService.UpsertFitbitSleeps(fitbitSleeps);
        }

    }
}
