﻿using System.Linq;
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
        
        public NokiaMigrator(IHealthService healthService, ILogger logger, INokiaClient nokiaClient)
        {
            _healthService = healthService;
            _logger = logger;
            _nokiaClient = nokiaClient;
        }
        
        public async Task MigrateWeights()
        {
            var latestWeightDate  = _healthService.GetLatestWeightDate();
            _logger.Log($"Latest Weight record has a date of : {latestWeightDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var fromDate = latestWeightDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            _logger.Log($"Retrieving Weight records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var weights = await _nokiaClient.GetWeights(fromDate);
            _logger.Log($"Found {weights.Count()} weight records.");

            await _healthService.UpsertWeights(weights);
        }

        public async Task MigrateBloodPressures()
        {
            var latestBloodPressureDate = _healthService.GetLatestBloodPressureDate();
            _logger.Log($"Latest Blood Pressure record has a date of : {latestBloodPressureDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var fromDate = latestBloodPressureDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            _logger.Log($"Retrieving Blood Pressure records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {fromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var bloodPressures = await _nokiaClient.GetBloodPressures(fromDate);
            _logger.Log($"Found {bloodPressures.Count()} Blood Pressure records.");
            
            await _healthService.UpsertBloodPressures(bloodPressures);
        }

    }
}