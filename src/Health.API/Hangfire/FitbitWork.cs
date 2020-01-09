using System;
using System.Linq;
using System.Threading.Tasks;
using Fitbit;
using Services.Health;
using Utils;

namespace HealthAPI.Hangfire
{
    public class FitbitWork : IFitbitWork
    {
        private ILogger _logger;
        private IHealthService _healthService;
        private ICalendar _calendar;
        private IFitbitService _fitbitService;

        private const int SEARCH_DAYS_PREVIOUS = 10;

        private DateTime MIN_FITBIT_DATE = new DateTime(2017, 5, 1);
        private DateTime MIN_FITBIT_SLEEP_DATE = new DateTime(2019, 7, 1);

        public FitbitWork(ILogger logger, IHealthService healthService, ICalendar calendar, IFitbitService fitbitService)
        {
            _logger = logger;
            _healthService = healthService;
            _calendar = calendar;
            _fitbitService = fitbitService;
        }


        public async Task ImportRestingHeartRates()
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

        public async Task ImportSleepSummaries()
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