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
                _logger.Log($"About to save Weight record : {weight.DateTime:yy-MM-dd} , {weight.Kg} Kg , {weight.FatRatioPercentage} % Fat");
                
                var existingWeight = await _healthRepository.FindAsync(weight);

                if (existingWeight == null)
                {
                    _healthRepository.Insert(weight);
                }
                else
                {
                    _healthRepository.Update(existingWeight, weight);
                }

            }
            
            
        }



        public async Task UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures)
        {
            foreach (var bloodPressure in bloodPressures)
            {
                _logger.Log($"About to save Blood Pressure record : {bloodPressure.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {bloodPressure.Diastolic} mmHg Diastolic , {bloodPressure.Systolic} mmHg Systolic");

                var existingBloodPressure = await _healthRepository.FindAsync(bloodPressure);
                if (existingBloodPressure != null)
                {
                    _healthRepository.Update(existingBloodPressure, bloodPressure);
                }
                else
                {
                    _healthRepository.Insert(bloodPressure);
                }
            }
            
            AddMovingAveragesToBloodPressures();

            _healthContext.SaveChanges();
        }

        public async Task UpsertStepCounts(IEnumerable<StepCount> stepCounts)
        {
            foreach (var stepCount in stepCounts)
            {
                var existingStepCount = await _healthRepository.FindAsync(stepCount);
                if (existingStepCount != null)
                {
                    _healthRepository.Update(existingStepCount, stepCount);
                }
                else
                {
                    _logger.Log($"Saving Step Data for {stepCount.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} : {stepCount.Count} steps");
                    _healthRepository.Insert(stepCount);
                }
            }

        }
        
        public async Task UpsertDailyActivities(IEnumerable<ActivitySummary>  dailyActivities)
        {
            foreach (var dailyActivity in dailyActivities)
            {
                var existingDailyActivity = await _healthRepository.FindAsync(dailyActivity);
                if (existingDailyActivity != null)
                {
                    _healthRepository.Update(existingDailyActivity, dailyActivity);
                }
                else
                {
                    _logger.Log($"Saving Activity Data for {dailyActivity.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} : {dailyActivity.SedentaryMinutes} sedentary minutes, {dailyActivity.LightlyActiveMinutes} lightly active minutes, {dailyActivity.FairlyActiveMinutes} fairly active minutes, {dailyActivity.VeryActiveMinutes} very active minutes.");
                    _healthRepository.Insert(dailyActivity);
                }
            }

        }
        
        public async Task UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates)
        {
            foreach (var restingHeartRate in restingHeartRates)
            {
                _logger.Log($"About to save Resting Heart Rate record : {restingHeartRate.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {restingHeartRate.Beats} beats");
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