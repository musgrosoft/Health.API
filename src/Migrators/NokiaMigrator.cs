using System;
using System.Linq;
using System.Threading.Tasks;
using Services.MyHealth;
using Services.Nokia;
using Utils;

namespace Migrators
{   
    public class NokiaMigrator
    {
        private readonly IHealthService _healthService;
        private readonly ILogger _logger;
        private readonly INokiaClient _nokiaClient;

        private const int SEARCH_DAYS_PREVIOUS = 10;

        private DateTime MIN_WEIGHT_DATE = new DateTime(2012, 1, 1);
        private DateTime MIN_BLOOD_PRESSURE_DATE = new DateTime(2012, 1, 1);

        public NokiaMigrator(IHealthService healthService, ILogger logger, INokiaClient nokiaClient)
        {
            _healthService = healthService;
            _logger = logger;
            _nokiaClient = nokiaClient;
        }
        
        public async Task MigrateWeights()
        {
            var latestWeightDate  = _healthService.GetLatestWeightDate(MIN_WEIGHT_DATE);
            _logger.Log($"WEIGHT : Latest Weight record has a date of : {latestWeightDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var fromDate = latestWeightDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            
            var weights = await _nokiaClient.GetWeights(fromDate);
            _logger.Log($"WEIGHT : Found {weights.Count()} weight records, in previous {SEARCH_DAYS_PREVIOUS} days ");

            _healthService.UpsertWeights(weights);
          //  _healthService.AddMovingAveragesToWeights();
        }

        public async Task MigrateBloodPressures()
        {
            var latestBloodPressureDate = _healthService.GetLatestBloodPressureDate(MIN_BLOOD_PRESSURE_DATE);
            _logger.Log($"BLOOD PRESSURE : Latest Blood Pressure record has a date of : {latestBloodPressureDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var fromDate = latestBloodPressureDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            _logger.Log($"BLOOD PRESSURE : Retrieving Blood Pressure records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var bloodPressures = await _nokiaClient.GetBloodPressures(fromDate);
            _logger.Log($"BLOOD PRESSURE : Found {bloodPressures.Count()} Blood Pressure records.");
            
            _healthService.UpsertBloodPressures(bloodPressures);

       //     _healthService.AddMovingAveragesToBloodPressures();

            
        }

    }
}
