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
        

        public void UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures)
        {
            _logger.Log($"BLOOD PRESSURE : Saving {bloodPressures.Count()} blood pressure");

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
        
        public void UpsertActivitySummaries(IEnumerable<ActivitySummary>  activitySummaries)
        {
            _logger.Log($"ACTIVITY SUMMARY : Saving {activitySummaries.Count()} Activity Summary");

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
        }

        public void UpsertHeartSummaries(IEnumerable<HeartSummary> heartSummaries)
        {
            _logger.Log($"HEART SUMMARY : Saving {heartSummaries.Count()} heart summaries");

            foreach (var heartSummary in heartSummaries)
            {
                
                var existingHeartSummary = _healthRepository.Find(heartSummary);
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
                _healthContext.SaveChanges();
            }

            
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
                _healthContext.SaveChanges();
            }

        }


        public void AddMovingAveragesToRestingHeartRates(int period = 10)
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
                _healthContext.SaveChanges();
            }

            
        }

        public void CalculateCumSumForStepCounts()
        {
            _logger.Log("STEP COUNTS : Calculate cum sum");
            var stepCounts = _healthContext.StepCounts.OrderBy(x => x.DateTime).ToList();
            for (int i = 0; i < stepCounts.Count(); i++)
            {
                if (i == 0)
                {
                    stepCounts[i].CumSumCount = stepCounts[i].Count;
                }
                else
                {
                    stepCounts[i].CumSumCount = stepCounts[i].Count + stepCounts[i - 1].CumSumCount;
                }
                _healthContext.SaveChanges();
            }

            

        }


        public void CalculateCumSumForUnits()
        {
            _logger.Log("UNITS : Calculate cum sum");
            var alcoholIntakes = _healthContext.AlcoholIntakes.OrderBy(x => x.DateTime).ToList();
            for (int i = 0; i < alcoholIntakes.Count(); i++)
            {
                if (i == 0)
                {
                    alcoholIntakes[i].CumSumUnits = alcoholIntakes[i].Units;
                }
                else
                {
                    alcoholIntakes[i].CumSumUnits = alcoholIntakes[i].Units + alcoholIntakes[i - 1].CumSumUnits;
                }
                _healthContext.SaveChanges();
            }

            
        }

        public void CalculateCumSumForActivitySummaries()
        {
            _logger.Log("ACTIVITY SUMMARY : Calculate cum sum");
            var activitySummaries = _healthContext.ActivitySummaries.OrderBy(x => x.DateTime).ToList();
            for (int i = 0; i < activitySummaries.Count(); i++)
            {
                if (i == 0)
                {
                    activitySummaries[i].CumSumActiveMinutes = activitySummaries[i].ActiveMinutes;
                }
                else
                {
                    activitySummaries[i].CumSumActiveMinutes = activitySummaries[i].ActiveMinutes + activitySummaries[i - 1].CumSumActiveMinutes;
                }
                _healthContext.SaveChanges();
            }

            
        }

        public void CalculateCumSumForHeartSummaries()
        {
            _logger.Log("HEART SUMMARY : Calculate cum sum");
            var heartSummaries = _healthContext.HeartSummaries.OrderBy(x => x.DateTime).ToList();
            for (int i = 0; i < heartSummaries.Count(); i++)
            {
                if (i == 0)
                {
                    heartSummaries[i].CumSumFatBurnAndAbove = heartSummaries[i].FatBurnMinutes + heartSummaries[i].CardioMinutes + heartSummaries[i].PeakMinutes;
                    heartSummaries[i].CumSumCardioAndAbove = heartSummaries[i].CardioMinutes + heartSummaries[i].PeakMinutes;
                }
                else
                {
                    heartSummaries[i].CumSumFatBurnAndAbove = heartSummaries[i].FatBurnMinutes + heartSummaries[i].CardioMinutes + heartSummaries[i].PeakMinutes + heartSummaries[i - 1].CumSumFatBurnAndAbove;
                    heartSummaries[i].CumSumCardioAndAbove = heartSummaries[i].CardioMinutes + heartSummaries[i].PeakMinutes + heartSummaries[i - 1].CumSumCardioAndAbove;
                }
                _healthContext.SaveChanges();
            }

            
        }

    }
}
