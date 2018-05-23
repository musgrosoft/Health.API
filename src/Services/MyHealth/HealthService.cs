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
        
        public void UpsertWeights(IEnumerable<Weight> weights)
        {
            _logger.Log($"WEIGHT : Saving {weights.Count()} weight");

            foreach (var weight in weights)
            {   
                var existingWeight = _healthRepository.Find(weight);

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
            _logger.Log($"BLOOD PRESSURE : Saving {bloodPressures.Count()} blood pressure");

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
            _logger.Log($"STEP COUNT : Saving {stepCounts.Count()} Step Count");

            foreach (var stepCount in stepCounts)
            {
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
        
        public async Task UpsertDailyActivities(IEnumerable<ActivitySummary>  activitySummaries)
        {
            _logger.Log($"ACTIVITY SUMMARY : Saving {activitySummaries.Count()} Activity Summary");

            foreach (var activitySummary in activitySummaries)
            {
               
                var existingDailyActivity = await _healthRepository.FindAsync(activitySummary);
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
        
        public async Task UpsertRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates)
        {
            _logger.Log($"RESTING HEART RATE : Saving {restingHeartRates.Count()} resting heart rates");

            foreach (var restingHeartRate in restingHeartRates)
            {
                var existingRestingHeartRate = await _healthRepository.FindAsync(restingHeartRate);
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
        }

        public async Task UpsertDailyHeartSummaries(IEnumerable<HeartSummary> heartSummaries)
        {
            _logger.Log($"HEART SUMMARY : Saving {heartSummaries.Count()} heart summaries");

            foreach (var heartSummary in heartSummaries)
            {
                
                var existingHeartSummary = await _healthRepository.FindAsync(heartSummary);
                if (existingHeartSummary != null)
                {
                    _logger.Log("HEART SUMMARY : update thing");
                    _healthRepository.Update(existingHeartSummary, heartSummary);
                }
                else
                {
                    _logger.Log("HEART SUMMARY : insert thing");
                    _healthRepository.Insert(heartSummary);
                }
            }
        }

        public void AddMovingAveragesToWeights(int period = 10)
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

            _healthContext.SaveChanges();
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