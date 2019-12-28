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

        private const int SEARCH_DAYS_PREVIOUS = 10;

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
                //await MigrateSleepStates();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(ex);
            }
        }

        private async Task MigrateSleepStates()
        {
            var latestFitbitSleepStateDate = _healthService.GetLatestSleepStateDate(MIN_FITBIT_SLEEP_DATE);
            var fromDate = latestFitbitSleepStateDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            await _logger.LogMessageAsync($"SLEEP STATES : Latest Sleep State record has a date of : {latestFitbitSleepStateDate:dd-MMM-yyyy HH:mm:ss (ddd)}, retrieving records from {SEARCH_DAYS_PREVIOUS} days previous : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}.");

            var sleepStates = await _fitbitService.GetSleepStates(fromDate, _calendar.Now());

            await _logger.LogMessageAsync($"SLEEP STATES : found {sleepStates.Count()} records.");

            if (sleepStates.Any())
            {
                await _logger.LogMessageAsync($"SLEEP STATES : First at {sleepStates.Min(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)} , last at {sleepStates.Max(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)}.");
            }

            await _healthService.UpsertAsync(sleepStates);

            await _logger.LogMessageAsync($"SLEEP STATES : Finished Importing.");
        }

        private async Task MigrateRestingHeartRates()
        {
            var latestRestingHeartRateDate = _healthService.GetLatestRestingHeartRateDate(MIN_FITBIT_DATE);
            var fromDate = latestRestingHeartRateDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            await _logger.LogMessageAsync($"RESTING HEART RATES : Latest record has a date of : {latestRestingHeartRateDate:dd-MMM-yyyy HH:mm:ss (ddd)}, retrieving records from {SEARCH_DAYS_PREVIOUS} days previous : {fromDate:dd-MMM-yyyy HH:mm:ss(ddd)}");

            var restingHeartRates = await _fitbitService.GetRestingHeartRates(fromDate, _calendar.Now());

            await _logger.LogMessageAsync($"RESTING HEART RATES : found {restingHeartRates.Count()} records");

            if (restingHeartRates.Any())
            {
                await _logger.LogMessageAsync($"RESTING HEART RATES : First at {restingHeartRates.Min(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)} , last at {restingHeartRates.Max(x => x.CreatedDate):dd-MMM-yyyy HH:mm:ss (ddd)}.");
            }

            await _healthService.UpsertAsync(restingHeartRates);

            await _logger.LogMessageAsync($"RESTING HEART RATES : Finished Importing.");

        }

        private async Task MigrateSleepSummaries()
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




    }
}