using System;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;
using Services.MyHealth;
using Services.Nokia;
using Utils;

namespace Migrators
{
    public class NokiaMigrator
    {
        private readonly HealthService _healthService;
        private readonly ILogger _logger;
        private readonly NokiaClient _nokiaClient;

        private const int SEARCH_DAYS_PREVIOUS = 10;

        
        
        public NokiaMigrator(HealthService healthService, ILogger logger, NokiaClient nokiaClient)
        {
            _healthService = healthService;
            _logger = logger;
            _nokiaClient = nokiaClient;
        }
        
        public async Task MigrateWeights()
        {
            var latestWeightDate  = _healthService.GetLatestWeightDate();
            _logger.Log($"Latest Weight record has a date of : {latestWeightDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestWeightDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            _logger.Log($"Retrieving Weight records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var scaleMeasurements = await _nokiaClient.GetScaleMeasures(getDataFromDate);
            _logger.Log($"Found {scaleMeasurements.Count()} weight records.");

            var weights = scaleMeasurements.Select(x => new Weight { DateTime = x.DateTime, Kg = x.Kg, FatRatioPercentage = x.FatRatioPercentage});

            await _healthService.UpsertWeights(weights);

     //       await _healthService.AddMovingAveragesToBloodPressures();
        }

        public async Task MigrateBloodPressures()
        {
            var latestBloodPressureDate = _healthService.GetLatestBloodPressureDate();
            _logger.Log($"Latest Blood Pressure record has a date of : {latestBloodPressureDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestBloodPressureDate.AddDays(-SEARCH_DAYS_PREVIOUS);
            _logger.Log($"Retrieving Blood Pressure records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var bloodPressures = await _nokiaClient.GetBloodPressures(getDataFromDate);
            _logger.Log($"Found {bloodPressures.Count()} Blood Pressure records.");

            var bps = bloodPressures.Select(x => new BloodPressure
            {
                DateTime = x.DateTime,
                Diastolic = (int)x.Diastolic,
                Systolic = (int)x.Systolic
            });
            
            await _healthService.UpsertBloodPressures(bps);

            await _healthService.AddMovingAveragesToBloodPressures();
        }

     

    }
}
