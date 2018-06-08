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
            _logger.Log($"WEIGHT : Saving {weights.Count()} weight");

            var orderedWeights = weights.OrderBy(x => x.DateTime).ToList();

            var previousWeights = _healthRepository.GetLatestWeights(10, orderedWeights.Min(x => x.DateTime));

            SetMovingAveragesForWeights(previousWeights.Select(x=>x.Kg).ToList(), orderedWeights);

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
            _logger.Log($"STEP COUNT : Saving {stepCounts.Count()} Step Count");

            var orderedStepCounts = stepCounts.OrderBy(x => x.DateTime).ToList();

            var previousStepCount = _healthRepository.GetLatestStepCounts(1, orderedStepCounts.Min(x=>x.DateTime)).FirstOrDefault();

            SetCumSumsForStepCounts(previousStepCount?.CumSumCount, orderedStepCounts);

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

            SetCumSumsForActivitySummaries(previousActivitySummary?.CumSumActiveMinutes, orderedActivitySummaries);

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
            
            AddMovingAveragesToRestingHeartRates(latestRestingHeartRates);

            _healthRepository.SaveChanges();
        }

        public void UpsertHeartSummaries(IEnumerable<HeartSummary> heartSummaries)
        {
            _logger.Log($"HEART SUMMARY : Saving {heartSummaries.Count()} heart summaries");

            var orderedHeartSummaries = heartSummaries.OrderBy(x => x.DateTime).ToList();

            var previousHeartSummary = _healthRepository.GetLatestHeartSummaries(1, orderedHeartSummaries.Min(x => x.DateTime)).FirstOrDefault();

            SetCumSumsForHeartSummaries(previousHeartSummary?.CumSumCardioAndAbove , orderedHeartSummaries);

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



        public void AddMovingAverageTo<T>(
            IEnumerable<T> theList,
            Func<T, DateTime> dateTimeSelector,
            Func<T, Decimal> GetValue,
            Action<T, Decimal?> SetMovingAverage,
            int period = 10
        ) where T : class
        {
            var orderedList = theList.OrderBy(dateTimeSelector).ToList();

            for (int i = 0; i < orderedList.Count(); i++)
            {
                if (i >= period - 1)
                {
                    Decimal total = 0;
                    //to rewrite from i-period, ascending
                    for (int x = i; x > (i - period); x--)
                    {
                        total += GetValue(orderedList[x]);
                    }

                    decimal average = total / period;

                    SetMovingAverage(orderedList[i], average);
                }
                //else
                //{
                //    SetMovingAverage(orderedList[i], null);
                //}

            }

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


        public void SetMovingAveragesForWeights(List<decimal> kgs, List<Weight> orderedWeights, int period = 10)
        {

            for (int i = 0; i < orderedWeights.Count-1; i++)
            {
                Decimal total = 0;

                if (i < period-1)
                {
                    for (int j = kgs.Count-1; j > i; j--)
                    {
                        total += kgs[j];
                    }

                    for (int j = 0; j <= i; j++)
                    {
                        total += orderedWeights[j].Kg;
                    }

                }
                else 
                {
                    for (int j = i - period + 1; j <= i ; j++)
                    {
                        total += orderedWeights[j].Kg;
                    }
                }

                decimal average = total / period;

                orderedWeights[i].MovingAverageKg = average;
            }
        }

        private void SetCumSumsForStepCounts(int? seed, IList<StepCount> orderedStepCounts)
        {
            for (int i = 0; i < orderedStepCounts.Count; i++)
            {
                int? previousCumSum;

                if (i == 0)
                {
                    previousCumSum = (seed ?? 0);
                }
                else
                {
                    previousCumSum = orderedStepCounts[i - 1].CumSumCount;
                }

                orderedStepCounts[i].CumSumCount = (orderedStepCounts[i].Count ?? 0) + previousCumSum;
            }
       }

        private void SetCumSumsForActivitySummaries(int? seed, IList<ActivitySummary> orderedActivitySummaries)
        {
            for (int i = 0; i < orderedActivitySummaries.Count; i++)
            {
                int? previousCumSum;

                if (i == 0)
                {
                    previousCumSum = (seed ?? 0);
                }
                else
                {
                    previousCumSum = orderedActivitySummaries[i - 1].CumSumActiveMinutes;
                }

                orderedActivitySummaries[i].CumSumActiveMinutes = (orderedActivitySummaries[i].ActiveMinutes) + previousCumSum;
            }
        }

        private void SetCumSumsForHeartSummaries(int? seed, List<HeartSummary> orderedHeartSummaries)
        {
            for (int i = 0; i < orderedHeartSummaries.Count; i++)
            {
                int? previousCumSum;

                if (i == 0)
                {
                    previousCumSum = (seed ?? 0);
                }
                else
                {
                    previousCumSum = orderedHeartSummaries[i - 1].CumSumCardioAndAbove;
                }

                orderedHeartSummaries[i].CumSumCardioAndAbove = (orderedHeartSummaries[i].CardioMinutes) + previousCumSum;
            }
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
