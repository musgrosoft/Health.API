using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Repositories;
using Repositories.Health;
using Repositories.Models;
using Utils;

namespace Services.MyHealth
{
    public class HealthService : IHealthService
    {   
        private readonly IConfig _config;
        private readonly ILogger _logger;
        private readonly IHealthRepository _healthRepository;
        private readonly IAggregationCalculator _aggregationCalculator;
        private const int MOVING_AVERAGE_PERIOD = 10;

        public HealthService(
            IConfig config, 
            ILogger logger, 
            IHealthRepository healthRepository,
            IAggregationCalculator aggregationCalculator)
        {
            _config = config;
            _logger = logger;
            _healthRepository = healthRepository;
            _aggregationCalculator = aggregationCalculator;
        }

        public DateTime GetLatestWeightDate(DateTime defaultDateTime)
        {
            var latestWeightDate = _healthRepository.GetLatestWeightDate();
            return latestWeightDate ?? defaultDateTime;
        }

        public DateTime GetLatestBloodPressureDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestBloodPressureDate();
            return latestDate ?? defaultDateTime;
        }
        
        public DateTime GetLatestStepCountDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestStepCountDate();
            return latestDate ?? defaultDateTime;
        }
        
        public DateTime GetLatestActivitySummaryDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestActivitySummaryDate();
            return latestDate ?? defaultDateTime;
        }

        public DateTime GetLatestRestingHeartRateDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestRestingHeartRateDate();
            return latestDate ?? defaultDateTime;
        }

        public DateTime GetLatestHeartSummaryDate(DateTime defaultDateTime)
        {
            var latestDate = _healthRepository.GetLatestHeartSummaryDate();
            return latestDate ?? defaultDateTime;
        }
        
        public void UpsertWeights(IEnumerable<Weight> weights)
        {
            _logger.Log($"WEIGHT : Saving {weights.Count()} weight");

            var orderedWeights = weights.OrderBy(x => x.DateTime).ToList();

            var previousWeights = _healthRepository.GetLatestWeights(MOVING_AVERAGE_PERIOD - 1, orderedWeights.Min(x => x.DateTime)).ToList();

            _aggregationCalculator.SetMovingAveragesOnWeights(previousWeights, orderedWeights, MOVING_AVERAGE_PERIOD);

            foreach (var weight in weights)
            {
                _healthRepository.Upsert(weight);
            }

        }

        public void UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures)
        {
            var countBloodPressures = bloodPressures.Count();

            _logger.Log($"BLOOD PRESSURE : Saving {bloodPressures.Count()} blood pressure");

            var orderedBloodPressures = bloodPressures.OrderBy(x => x.DateTime).ToList();

            var previousBloodPressures = _healthRepository.GetLatestBloodPressures(MOVING_AVERAGE_PERIOD-1, orderedBloodPressures.Min(x => x.DateTime)).ToList();

            _aggregationCalculator.SetMovingAveragesOnBloodPressures(previousBloodPressures, orderedBloodPressures, MOVING_AVERAGE_PERIOD);

            foreach (var bloodPressure in bloodPressures)
            {
                var existingBloodPressure = _healthRepository.Find(bloodPressure);

                if (existingBloodPressure != null)
                {
                    _logger.Log($"BLOOD PRESSURE : Updating record : {bloodPressure.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {bloodPressure.Diastolic} mmHg Diastolic , {bloodPressure.Systolic} mmHg Systolic");
                    _healthRepository.Update(existingBloodPressure, bloodPressure);
                }
                else
                {
                    _logger.Log($"BLOOD PRESSURE : Inserting record : {bloodPressure.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {bloodPressure.Diastolic} mmHg Diastolic , {bloodPressure.Systolic} mmHg Systolic");
                    _healthRepository.Insert(bloodPressure);
                }
            }

            //_logger.Log($"BLOOD PRESSURE : moving averages");

            //var latestBloodPressures = _healthRepository.GetLatestBloodPressures(countBloodPressures + 10).ToList();

            //_aggregationCalculator.AddMovingAveragesToBloodPressures(latestBloodPressures);

            //_healthRepository.SaveChanges();

        }

        public void UpsertStepCounts(IEnumerable<StepCount> stepCounts)
        {
            _logger.Log($"STEP COUNT : Saving {stepCounts.Count()} Step Count");

            var orderedStepCounts = stepCounts.OrderBy(x => x.DateTime).ToList();

            var previousStepCount = _healthRepository.GetLatestStepCounts(1, orderedStepCounts.Min(x=>x.DateTime)).FirstOrDefault();

            _aggregationCalculator.SetCumSumsOnStepCounts(previousStepCount?.CumSumCount, orderedStepCounts);

            for (int i = 0; i < orderedStepCounts.Count; i++)
            {
                var stepCount = orderedStepCounts[i];

                var existingStepCount = _healthRepository.Find(stepCount);
                if (existingStepCount != null)
                {
                    _logger.Log($"STEP COUNT : Update Step Data for {stepCount.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} : {stepCount.Count} steps");
                    _healthRepository.Update(existingStepCount, stepCount);
                }
                else
                {
                    _logger.Log($"STEP COUNT : Insert Step Data for {stepCount.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} : {stepCount.Count} steps");
                    _healthRepository.Insert(stepCount);
                }
            }
            
        }



        public void UpsertActivitySummaries(IEnumerable<ActivitySummary>  activitySummaries)
        {
            _logger.Log($"ACTIVITY SUMMARY : Saving {activitySummaries.Count()} Activity Summary");

            var orderedActivitySummaries = activitySummaries.OrderBy(x => x.DateTime).ToList();

            var previousActivitySummary = _healthRepository.GetLatestActivitySummaries(1, orderedActivitySummaries.Min(x => x.DateTime)).FirstOrDefault();

            _aggregationCalculator.SetCumSumsOnActivitySummaries(previousActivitySummary?.CumSumActiveMinutes, orderedActivitySummaries);

            foreach (var activitySummary in activitySummaries)
            {
               
                var existingDailyActivity = _healthRepository.Find(activitySummary);
                if (existingDailyActivity != null)
                {
                     _logger.Log($"ACTIVITY SUMMARY : Update Activity Data for {activitySummary.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} : {activitySummary.SedentaryMinutes} sedentary minutes, {activitySummary.LightlyActiveMinutes} lightly active minutes, {activitySummary.FairlyActiveMinutes} fairly active minutes, {activitySummary.VeryActiveMinutes} very active minutes.");
                    _healthRepository.Update(existingDailyActivity, activitySummary);
                }
                else
                {
                    _logger.Log($"ACTIVITY SUMMARY : Insert Activity Data for {activitySummary.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} : {activitySummary.SedentaryMinutes} sedentary minutes, {activitySummary.LightlyActiveMinutes} lightly active minutes, {activitySummary.FairlyActiveMinutes} fairly active minutes, {activitySummary.VeryActiveMinutes} very active minutes.");
                    _healthRepository.Insert(activitySummary);
                }
            }

        }
        
        public void UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates)
        {
            var countRestingHeartRates = restingHeartRates.Count();

            _logger.Log($"RESTING HEART RATE : Saving {restingHeartRates.Count()} resting heart rates");
            
            foreach (var restingHeartRate in restingHeartRates)
            {
                var existingRestingHeartRate = _healthRepository.Find(restingHeartRate);
                if (existingRestingHeartRate != null)
                {
                    _logger.Log($"RESTING HEART RATE : About to update Resting Heart Rate record : {restingHeartRate.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {restingHeartRate.Beats} beats");
                    _healthRepository.Update(existingRestingHeartRate, restingHeartRate);
                }
                else
                {
                    _logger.Log($"RESTING HEART RATE : About to insert Resting Heart Rate record : {restingHeartRate.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {restingHeartRate.Beats} beats");
                    _healthRepository.Insert(restingHeartRate);
                }
            }

            _logger.Log($"RESTING HEART RATE : Moving averages");

            var latestRestingHeartRates = _healthRepository.GetLatestRestingHeartRates(countRestingHeartRates + 10).ToList();

            _aggregationCalculator.SetMovingAveragesOnRestingHeartRates(latestRestingHeartRates);

            //_healthRepository.SaveChanges();
        }

        public void UpsertHeartSummaries(IEnumerable<HeartSummary> heartSummaries)
        {
            _logger.Log($"HEART SUMMARY : Saving {heartSummaries.Count()} heart summaries");

            var orderedHeartSummaries = heartSummaries.OrderBy(x => x.DateTime).ToList();

            var previousHeartSummary = _healthRepository.GetLatestHeartSummaries(1, orderedHeartSummaries.Min(x => x.DateTime)).FirstOrDefault();

            _aggregationCalculator.SetCumSumsOnHeartSummaries(previousHeartSummary?.CumSumCardioAndAbove , orderedHeartSummaries);

            foreach (var heartSummary in heartSummaries)
            {
                
                var existingHeartSummary = _healthRepository.Find(heartSummary);
                if (existingHeartSummary != null)
                {
                    _logger.Log($"HEART SUMMARY : About to update Heart SUmmary Record : {heartSummary.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , ");
                    _healthRepository.Update(existingHeartSummary, heartSummary);
                }
                else
                {
                    _logger.Log("HEART SUMMARY : insert thing");
                    _healthRepository.Insert(heartSummary);
                }
            }

        }


        public void UpsertAlcoholIntakes()
        {
            _logger.Log("UNITS : Calculate cum sum");

            var allAlcoholIntakes = _healthRepository.GetAllAlcoholIntakes().ToList();

            _aggregationCalculator.SetCumSumsOnAlcoholIntakes(allAlcoholIntakes);

            //_healthRepository.SaveChanges();
            

        }


        




    }
}
