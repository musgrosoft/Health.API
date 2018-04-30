using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Services.MyHealth;
using Services.MyHealth.Domain;
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

        private DateTime MIN_BLOOD_PRESSURE_DATE = new DateTime(2012, 1, 1);
        private DateTime MIN_WEIGHT_DATE = new DateTime(2012, 1, 1);

        public NokiaMigrator(HealthService healthService, ILogger logger, NokiaClient nokiaClient)
        {
            _healthService = healthService;
            _logger = logger;
            _nokiaClient = nokiaClient;
        }
        
        public async Task MigrateWeights()
        {
            var latestWeight = await _healthService.GetLatestWeight();
            var latestWeightDate = latestWeight?.DateTime ?? MIN_WEIGHT_DATE;

            _logger.Log($"Latest Weight record has a date of : {latestWeightDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestWeightDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            _logger.Log($"Retrieving Weight records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var scaleMeasurements = await _nokiaClient.GetScaleMeasures(getDataFromDate);

            _logger.Log($"Found {scaleMeasurements.Count()} weight records.");

            foreach (var dataScaleMeasure in scaleMeasurements)
            {
                var wsData = new Weight
                {
                    DateTime = dataScaleMeasure.DateTime,
                    Kg = dataScaleMeasure.Kg,
                    FatRatioPercentage = dataScaleMeasure.FatRatioPercentage
                };

                _logger.Log($"About to save Weight record : {wsData.DateTime:yy-MM-dd} , {wsData.Kg} Kg , {wsData.FatRatioPercentage} % Fat");

                await _healthService.SaveWeight(wsData);
            }

            await _healthService.AddMovingAveragesToWeights();
        }


       


        public async Task MigrateBloodPressures()
        {
            var latestBloodPressure = await _healthService.GetLatestBloodPressure();
            var latestBloodPressureDate = latestBloodPressure?.DateTime ?? MIN_BLOOD_PRESSURE_DATE;

            _logger.Log($"Latest Blood Pressure record has a date of : {latestBloodPressureDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var getDataFromDate = latestBloodPressureDate.AddDays(-SEARCH_DAYS_PREVIOUS);

            _logger.Log($"Retrieving Blood Pressure records from {SEARCH_DAYS_PREVIOUS} days previous to last record. Retrieving from date : {getDataFromDate:dd-MMM-yyyy HH:mm:ss (ddd)}");

            var bloodPressures = await _nokiaClient.GetBloodPressures(getDataFromDate);

            _logger.Log($"Found {bloodPressures.Count()} Blood Pressure records.");

            foreach (var bloodPressure in bloodPressures)
            {
                var bpData = new BloodPressure
                {
                    DateTime = bloodPressure.DateTime,
                    Diastolic = (int)bloodPressure.Diastolic,
                    Systolic = (int)bloodPressure.Systolic
                };

                _logger.Log($"About to save Blood Pressure record : {bpData.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {bpData.Diastolic} mmHg Diastolic , {bpData.Systolic} mmHg Systolic");

                await _healthService.SaveBloodPressure(bpData);
            }

            await _healthService.AddMovingAveragesToBloodPressures();
        }

     

    }
}
