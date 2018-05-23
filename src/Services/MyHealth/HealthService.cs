using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly HealthContext _healthContext;
        private readonly IHealthRepository _healthRepository;

        public HealthService(IConfig config, ILogger logger, HealthContext healthContext, IHealthRepository healthRepository)
        {
            _config = config;
            _logger = logger;
            _healthContext = healthContext;
            _healthRepository = healthRepository;
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
        
        public async Task UpsertWeights(IEnumerable<Weight> weights)
        {
            foreach (var weight in weights)
            {   
                var existingWeight = await _healthRepository.FindAsync(weight);

                if (existingWeight == null)
                {
                    _logger.Log($"WEIGHT : Insert Weight record : {weight.DateTime:yy-MM-dd} , {weight.Kg} Kg , {weight.FatRatioPercentage} % Fat");
                    _healthRepository.Insert(weight);
                }
                else
                {
                    _logger.Log($"WEIGHT : Update Weight record : {weight.DateTime:yy-MM-dd} , {weight.Kg} Kg , {weight.FatRatioPercentage} % Fat");
                    _healthRepository.Update(existingWeight, weight);
                }
            }
        }
        

        public async Task UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures)
        {
            foreach (var bloodPressure in bloodPressures)
            {

                var existingBloodPressure = await _healthRepository.FindAsync(bloodPressure);

                if (existingBloodPressure != null)
                {
                    _logger.Log($"BLOOD PRESSURE : About to update Blood Pressure record : {bloodPressure.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {bloodPressure.Diastolic} mmHg Diastolic , {bloodPressure.Systolic} mmHg Systolic");
                    _healthRepository.Update(existingBloodPressure, bloodPressure);
                }
                else
                {
                    _logger.Log($"BLOOD PRESSURE : About to insert Blood Pressure record : {bloodPressure.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {bloodPressure.Diastolic} mmHg Diastolic , {bloodPressure.Systolic} mmHg Systolic");
                    _healthRepository.Insert(bloodPressure);
                }
            }
            
            AddMovingAveragesToBloodPressures();

            _healthContext.SaveChanges();
        }

        public void UpsertStepCounts(IEnumerable<StepCount> stepCounts)
        {
            foreach (var stepCount in stepCounts)
            {
                _logger.Log($"STEP COUNT : Saving Step Data for {stepCount.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} : {stepCount.Count} steps");
                var existingStepCount = _healthRepository.Find(stepCount);
                if (existingStepCount != null)
                {
                    _healthRepository.Update(existingStepCount, stepCount);
                }
                else
                {
                    
                    _healthRepository.Insert(stepCount);
                }
            }
        }
        
        public async Task UpsertDailyActivities(IEnumerable<ActivitySummary>  dailyActivities)
        {
            foreach (var dailyActivity in dailyActivities)
            {
                _logger.Log($"ACTIVITY SUMMARY : Saving Activity Data for {dailyActivity.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} : {dailyActivity.SedentaryMinutes} sedentary minutes, {dailyActivity.LightlyActiveMinutes} lightly active minutes, {dailyActivity.FairlyActiveMinutes} fairly active minutes, {dailyActivity.VeryActiveMinutes} very active minutes.");
                var existingDailyActivity = await _healthRepository.FindAsync(dailyActivity);
                if (existingDailyActivity != null)
                {
                    _healthRepository.Update(existingDailyActivity, dailyActivity);
                }
                else
                {
                    _healthRepository.Insert(dailyActivity);
                }
            }
        }
        
        public async Task UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates)
        {
            foreach (var restingHeartRate in restingHeartRates)
            {
                _logger.Log($"RESTING HEART RATE : About to save Resting Heart Rate record : {restingHeartRate.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {restingHeartRate.Beats} beats");
                var existingRestingHeartRate = await _healthRepository.FindAsync(restingHeartRate);
                if (existingRestingHeartRate != null)
                {
                    _healthRepository.Update(existingRestingHeartRate, restingHeartRate);
                }
                else
                {
                    _healthRepository.Insert(restingHeartRate);
                }
            }
        }

        public async Task UpsertDailyHeartSummaries(IEnumerable<HeartSummary> heartSummaries)
        {
            foreach (var heartSummary in heartSummaries)
            {
                _logger.Log("HEART SUMMARY : saving thing");
                var existingHeartSummary = await _healthRepository.FindAsync(heartSummary);
                if (existingHeartSummary != null)
                {
                    _healthRepository.Update(existingHeartSummary, heartSummary);
                }
                else
                {
                    _healthRepository.Insert(heartSummary);
                }
            }
        }

        public async Task AddMovingAveragesToWeights(int period = 10)
        {
            _logger.Log("WEIGHT : Add moving averages");
            var orderedWeights = _healthContext.Weights.OrderBy(x => x.DateTime).ToList();

            for (int i = 0; i < orderedWeights.Count(); i++)
            {
                if (i >= period - 1)
                {
                    decimal total = 0;
                    for (int x = i; x > (i - period); x--)
                        total += orderedWeights[x].Kg;
                    decimal average = total / period;

                    orderedWeights[i].MovingAverageKg = average;
                }
                else
                {
                    orderedWeights[i].MovingAverageKg = null;
                }

            }

            await _healthContext.SaveChangesAsync();
        }


        public void AddMovingAveragesToBloodPressures(int period = 10)
        {
            _logger.Log("BLOOD PRESSURE : Add moving averages");
            var bloodPressures = _healthContext.BloodPressures.OrderBy(x => x.DateTime).ToList();

            for (int i = 0; i < bloodPressures.Count(); i++)
            {
                if (i >= period - 1)
                {
                    int systolicTotal = 0;
                    int diastolicTotal = 0;
                    for (int x = i; x > (i - period); x--)
                    {
                        systolicTotal += bloodPressures[x].Systolic;
                        diastolicTotal += bloodPressures[x].Diastolic;
                    }
                    int averageSystolic = systolicTotal / period;
                    int averageDiastolic = diastolicTotal / period;
                    bloodPressures[i].MovingAverageSystolic = averageSystolic;
                    bloodPressures[i].MovingAverageDiastolic = averageDiastolic;
                }
                else
                {
                    bloodPressures[i].MovingAverageSystolic = null;
                    bloodPressures[i].MovingAverageDiastolic = null;
                }
            }

        }


        public async Task AddMovingAveragesToRestingHeartRates(int period = 10)
        {
            _logger.Log("RESTING HEART RATE : Add moving averages");
            var heartRates = _healthContext.RestingHeartRates.OrderBy(x => x.DateTime).ToList();

            for (int i = 0; i < heartRates.Count(); i++)
            {
                if (i >= period - 1)
                {
                    decimal total = 0;
                    for (int x = i; x > (i - period); x--)
                        total += heartRates[x].Beats;
                    decimal average = total / period;

                    heartRates[i].MovingAverageBeats = average;
                }
                else
                {
                    heartRates[i].MovingAverageBeats = null;
                }
            }

            await _healthContext.SaveChangesAsync();
        }




    }
}