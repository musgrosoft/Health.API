using System;
using System.Linq;
using System.Threading.Tasks;
using Nokia.Services;
using Services.Health;
using Utils;

namespace Nokia.Migrator
{   
    public class NokiaMigrator : INokiaMigrator
    {
        private readonly IHealthService _healthService;
        private readonly ILogger _logger;
        private readonly INokiaService _nokiaService;
        private readonly ITargetService _targetService;

        private const int SEARCH_DAYS_PREVIOUS = 10;

        private DateTime MIN_WEIGHT_DATE = new DateTime(2012, 1, 1);
        private DateTime MIN_BLOOD_PRESSURE_DATE = new DateTime(2012, 1, 1);

        public NokiaMigrator(IHealthService healthService, ILogger logger, INokiaService nokiaService, ITargetService targetService)
        {
            _healthService = healthService;
            _logger = logger;
            _nokiaService = nokiaService;
            _targetService = targetService;
        }
        
        public async Task MigrateWeights()
        {
            var latestWeightDate  = _healthService.GetLatestWeightDate(MIN_WEIGHT_DATE);
            await _logger.LogMessageAsync($"WEIGHT : Latest Weight record has a date of : {latestWeightDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var fromDate = latestWeightDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            
            var weights = (await _nokiaService.GetWeights(fromDate)).ToList();
            await _logger.LogMessageAsync($"WEIGHT : Found {weights.Count()} weight records, in previous {SEARCH_DAYS_PREVIOUS} days ");

            weights = _targetService.SetTargets(weights);
            
            _healthService.UpsertWeights(weights);
        }

        public async Task MigrateBloodPressures()
        {
            var latestBloodPressureDate = _healthService.GetLatestBloodPressureDate(MIN_BLOOD_PRESSURE_DATE);
            await _logger.LogMessageAsync($"BLOOD PRESSURE : Latest Blood Pressure record has a date of : {latestBloodPressureDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var fromDate = latestBloodPressureDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            await _logger.LogMessageAsync($"BLOOD PRESSURE : Retrieving Blood Pressure records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var bloodPressures = await _nokiaService.GetBloodPressures(fromDate);
            await _logger.LogMessageAsync($"BLOOD PRESSURE : Found {bloodPressures.Count()} Blood Pressure records.");
            
            _healthService.UpsertBloodPressures(bloodPressures);
        }

    }
}
