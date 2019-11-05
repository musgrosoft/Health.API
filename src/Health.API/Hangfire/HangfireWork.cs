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
            var latestFitbitSleepSummaryDate = _healthService.GetLatestSleepSummaryDate(MIN_FITBIT_SLEEP_DATE);
            await _logger.LogMessageAsync($"SLEEP SUMMARIES : Latest Sleep Summary record has a date of : {latestFitbitSleepSummaryDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestFitbitSleepSummaryDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            
            await _logger.LogMessageAsync($"SLEEP SUMMARIES : Retrieving Sleep records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var fitbitSleeps = await _fitbitService.GetSleepSummaries(getDataFromDate, _calendar.Now());

            _healthService.UpsertSleepSummaries(fitbitSleeps);
        }


        private async Task MigrateSleepStates()
        {
            var latestFitbitSleepStateDate = _healthService.GetLatestSleepStateDate(MIN_FITBIT_SLEEP_DATE);
            await _logger.LogMessageAsync($"SLEEP STATES : Latest Sleep State record has a date of : {latestFitbitSleepStateDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestFitbitSleepStateDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            await _logger.LogMessageAsync($"SLEEP STATES : Retrieving Sleep State records from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");
            
            var sleepStates = await _fitbitService.GetSleepStates(getDataFromDate, _calendar.Now());

            await _logger.LogMessageAsync($"SLEEP STATES : found {sleepStates.Count()} ");

            if (sleepStates.Any())
            {
                await _logger.LogMessageAsync($"SLEEP STATES : First at {sleepStates.Min(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)} , last at {sleepStates.Max(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)}");
            }

            await _logger.LogMessageAsync($"SLEEP STATES : Upserting {sleepStates.Count()} records.");

            _healthService.UpsertSleepStates(sleepStates);

            await _logger.LogMessageAsync($"SLEEP STATES : Finished Importing Sleep States.");

        }

    }
}