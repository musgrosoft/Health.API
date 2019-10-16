using System;
using System.Linq;
using System.Threading.Tasks;
using Fitbit;
using Services.Health;
using Utils;

namespace HealthAPI.Hangfire
{
    public class HangfireWork : IHangfireWork
    {
        private readonly ILogger _logger;
        private readonly IHealthService _healthService;
        private readonly ICalendar _calendar;

        private const int SEARCH_DAYS_PREVIOUS = 1;

        private DateTime MIN_FITBIT_DATE = new DateTime(2017, 5, 1);
        private DateTime MIN_FITBIT_SLEEP_DATE = new DateTime(2019, 7, 1);
        private IFitbitService _fitbitService;

        public HangfireWork(ILogger logger, IHealthService healthService, ICalendar calendar, IFitbitService fitbitService)
        {
            

            _logger = logger;
            _healthService = healthService;
            _calendar = calendar;
            _fitbitService = fitbitService; //new FitbitService(tokenService, httpClient, config, logger);
        }

        public async Task MigrateAllFitbitData()
        {
            try
            {
                await MigrateRestingHeartRates();
                await MigrateSleepSummaries();
                await MigrateSleepStates();
                await MigrateDrinks();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex);
            }
        }

        private async Task MigrateRestingHeartRates()
        {
            var latestRestingHeartRateDate = _healthService.GetLatestRestingHeartRateDate(MIN_FITBIT_DATE);
            await _logger.LogMessageAsync($"RESTING HEART RATE : Latest Resting Heart Rate record has a date of : {latestRestingHeartRateDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestRestingHeartRateDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            await _logger.LogMessageAsync($"RESTING HEART RATE : Retrieving Resting Heart Rate records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var restingHeartRates = await _fitbitService.GetRestingHeartRates(getDataFromDate, _calendar.Now());

            _healthService.UpsertRestingHeartRates(restingHeartRates);
        }

        private async Task MigrateSleepSummaries()
        {
            var latestFitbitSleepDate = _healthService.GetLatestFitbitSleepDate(MIN_FITBIT_SLEEP_DATE);
            await _logger.LogMessageAsync($"SLEEP : Latest Sleep record has a date of : {latestFitbitSleepDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestFitbitSleepDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            getDataFromDate = latestFitbitSleepDate.AddDays(-5);

            await _logger.LogMessageAsync($"SLEEP : Retrieving Sleep records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var fitbitSleeps = await _fitbitService.GetSleepSummaries(getDataFromDate, _calendar.Now());
            _healthService.UpsertSleepSummaries(fitbitSleeps);

        }


        private async Task MigrateSleepStates()
        {
            var date = MIN_FITBIT_SLEEP_DATE;


            var sleepStates = await _fitbitService.GetSleepStates(date, _calendar.Now());
            _healthService.UpsertSleepStates(sleepStates);
        }


        private async Task MigrateDrinks()
        {
            var latestDrinkDate = _healthService.GetLatestDrinkDate();
            //var latestDrinkDate = new DateTime(2019, 8, 19);

            var data = await _fitbitService.GetDrinks(latestDrinkDate.AddDays(-5), _calendar.Now());

            _healthService.UpsertAlcoholIntakes(data.ToList());

        }
    }
}