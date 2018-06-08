using System;
using System.Collections.Generic;
using System.Linq;
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
            var countWeights = weights.Count();

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

            _logger.Log($"WEIGHT : moving averages");

            var latestWeights = _healthRepository.GetLatestWeights(countWeights + 10).ToList();
            
            AddMovingAveragesToWeights(latestWeights);

            _healthRepository.SaveChanges();
        }
        

        public void UpsertBloodPressures(IEnumerable<BloodPressure> bloodPressures)
        {
            var countBloodPressures = bloodPressures.Count();

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

            _logger.Log($"BLOOD PRESSURE : moving averages");

            var latestBloodPressures = _healthRepository.GetLatestBloodPressures(countBloodPressures + 10).ToList();
            
            AddMovingAveragesToBloodPressures(latestBloodPressures);

            _healthRepository.SaveChanges();

        }

        public void UpsertStepCounts(IEnumerable<StepCount> stepCounts)
        {
            var countStepCounts = stepCounts.Count();

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

            _logger.Log($"STEP COUNT : Cum sums");

            var latestStepCounts = _healthRepository.GetLatestStepCounts(countStepCounts + 1).ToList();
            
            AddCumSumsToStepCounts(latestStepCounts);

            _healthRepository.SaveChanges();
        }



        public void UpsertActivitySummaries(IEnumerable<ActivitySummary>  activitySummaries)
        {
            var countActivitySummaries = activitySummaries.Count();

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

            _logger.Log($"ACTIVITY SUMMARY : Cum sums");

            var latestActivitySummaries = _healthRepository.GetLatestActivitySummaries(countActivitySummaries + 1).ToList();
            
            AddCumSumsToActivitySummaries(latestActivitySummaries);

            _healthRepository.SaveChanges();
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
            
            AddMovingAveragesToRestingHeartRates(latestRestingHeartRates);

            _healthRepository.SaveChanges();
        }

        public void UpsertHeartSummaries(IEnumerable<HeartSummary> heartSummaries)
        {
            var countHeartSummaries = heartSummaries.Count();

            _logger.Log($"HEART SUMMARY : Saving {heartSummaries.Count()} heart summaries");
            
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

            _logger.Log($"HEART SUMMARY : cum sums");

            var latestHeartSummaries = _healthRepository.GetLatestHeartSummaries(countHeartSummaries + 1).ToList();
            
            AddCumSumsToHeartSummaries(latestHeartSummaries);

            _healthRepository.SaveChanges();

        }

        public void UpsertAlcoholIntakes()
        {
            _logger.Log("UNITS : Calculate cum sum");

            var allAlcoholIntakes = _healthRepository.GetAllAlcoholIntakes();
            
            AddCumSumsToAlcoholIntakes(allAlcoholIntakes);

            _healthRepository.SaveChanges();
            

        }



        public void AddMovingAveragesToWeights(IEnumerable<Weight> weights)
        {
            _logger.Log("WEIGHT : Add moving averages (using generic method)");

            _aggregationCalculator.AddMovingAverageTo(
                weights, 
                w => w.DateTime, 
                w => w.Kg, 
                (w, d) => w.MovingAverageKg = d 
                );
            
        }


        public void AddMovingAveragesToBloodPressures(IEnumerable<BloodPressure> bloodPressures)
        {
            _logger.Log("BLOOD PRESSURE : Add moving averages (using generic method)");

            _aggregationCalculator.AddMovingAverageTo(
                bloodPressures,
                w => w.DateTime,
                w => w.Systolic,
                (w, d) => w.MovingAverageSystolic = d
                );

            _aggregationCalculator.AddMovingAverageTo(
                bloodPressures,
                w => w.DateTime,
                w => w.Diastolic,
                (w, d) => w.MovingAverageDiastolic = d
                );
        }
        
        public void AddMovingAveragesToRestingHeartRates(IEnumerable<RestingHeartRate> restingHeartRates)
        {
            _logger.Log("RESTING HEART RATE : Add moving averages (using generic method)");

            _aggregationCalculator.AddMovingAverageTo(
                restingHeartRates,
                w => w.DateTime,
                w => w.Beats,
                (w, d) => w.MovingAverageBeats = d
                );
        }

        private void AddCumSumsToStepCounts(List<StepCount> stepCounts)
        {
            _aggregationCalculator.CalculateCumSumFor(
                stepCounts,
                sc => sc.DateTime,
                sc => sc.Count,
                sc => sc.CumSumCount,
                (sc, val) => sc.CumSumCount = val
            );
        }

        public void AddCumSumsToActivitySummaries(IEnumerable<ActivitySummary> activitySummaries)
        {
            _aggregationCalculator.CalculateCumSumFor(
                activitySummaries,
                ac => ac.DateTime,
                ac => ac.ActiveMinutes,
                ac => ac.CumSumActiveMinutes,
                (ac, val) => ac.CumSumActiveMinutes = val
            );

            //var activitySummaries = _healthContext.ActivitySummaries.OrderBy(x => x.DateTime).ToList();
            //for (int i = 0; i < activitySummaries.Count(); i++)
            //{
            //    if (i == 0)
            //    {
            //        activitySummaries[i].CumSumActiveMinutes = activitySummaries[i].ActiveMinutes;
            //    }
            //    else
            //    {
            //        activitySummaries[i].CumSumActiveMinutes = activitySummaries[i].ActiveMinutes + activitySummaries[i - 1].CumSumActiveMinutes;
            //    }
            //    _healthContext.SaveChanges();
            //}


        }

        public void AddCumSumsToHeartSummaries(IEnumerable<HeartSummary> heartSummaries)
        {
            _aggregationCalculator.CalculateCumSumFor(
                heartSummaries,
                hs => hs.DateTime,
                hs => hs.CardioMinutes + hs.PeakMinutes,
                hs => hs.CumSumCardioAndAbove,
                (hs, val) => hs.CumSumCardioAndAbove = val
            );
        }

        public void AddCumSumsToAlcoholIntakes(IEnumerable<AlcoholIntake> alcoholIntakes)
        {
            _logger.Log("UNITS : Calculate cum sum");

            _aggregationCalculator.CalculateCumSumFor(
                alcoholIntakes,
                ai => ai.DateTime,
                ai => ai.Units,
                ai => ai.CumSumUnits,
                (ai, val) => ai.CumSumUnits = val
            );
        }





    }
}
