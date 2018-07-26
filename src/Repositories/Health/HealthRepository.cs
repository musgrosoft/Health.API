﻿using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.Models;

namespace Repositories.Health
{
    public class HealthRepository : IHealthRepository
    {
        private readonly HealthContext _healthContext;

        public HealthRepository(HealthContext healthContext)
        {
            _healthContext = healthContext;
        }
        
        public DateTime? GetLatestStepCountDate()
        {
            return _healthContext.StepCounts.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }
        
        public DateTime? GetLatestBloodPressureDate()
        {
            return _healthContext.BloodPressures.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }

        public DateTime? GetLatestWeightDate()
        {
           return _healthContext.Weights.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }

        public DateTime? GetLatestAlcoholIntakeDate()
        {
            return _healthContext.AlcoholIntakes.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }

        public DateTime? GetLatestActivitySummaryDate()
        {
            return _healthContext.ActivitySummaries.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }

        public DateTime? GetLatestRestingHeartRateDate()
        {
            return _healthContext.RestingHeartRates.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }
        
        public DateTime? GetLatestHeartSummaryDate()
        {
            return _healthContext.HeartRateSummaries.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }

        public DateTime? GetLatestRunDate()
        {
            return _healthContext.Runs.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }



        public IList<AlcoholIntake> GetAllAlcoholIntakes()
        {
            //to list to materialize entities
            return _healthContext.AlcoholIntakes.OrderBy(x => x.CreatedDate).ToList();
        }

        public IList<ActivitySummary> GetAllActivitySummaries()
        {
            //to list to materialize entities
            return _healthContext.ActivitySummaries.OrderBy(x => x.CreatedDate).ToList();
        }

        public IList<HeartRateSummary> GetAllHeartRateSummaries()
        {
            //to list to materialize entities
            return _healthContext.HeartRateSummaries.OrderBy(x => x.CreatedDate).ToList();
        }
        
        public IList<RestingHeartRate> GetAllRestingHeartRates()
        {
            //to list to materialize entities
            return _healthContext.RestingHeartRates.OrderBy(x=>x.CreatedDate).ToList();
        }

        public IList<StepCount> GetAllStepCounts()
        {
            //to list to materialize entities
            return _healthContext.StepCounts.OrderBy(x => x.CreatedDate).ToList();
        }

        public IList<Run> GetAllRuns()
        {
            //to list to materialize entities
            return _healthContext.Runs.OrderBy(x => x.CreatedDate).ToList();
        }

        public IList<Ergo> GetAllErgos()
        {
            //to list to materialize entities
            return _healthContext.Ergos.OrderBy(x => x.CreatedDate).ToList();
        }

        public IList<Weight> GetAllWeights()
        {
            ////to list to materialize entities
            //return _healthContext.Weights
            //    .GroupBy(x => x.CreatedDate.Date)
            //    .Select(g => new Weight
            //    {
            //        CreatedDate = g.Key.Date,
            //        Kg = g.Average(w => w.Kg)
            //    })
            //    .OrderBy(x => x.CreatedDate).ToList();

            var allWeights = _healthContext.Weights.ToList();

            return allWeights
                .GroupBy(x => x.CreatedDate.Date)
                .Select(g => new Weight
                {
                    CreatedDate = g.Key.Date,
                    Kg = g.Average(w => w.Kg)
                })
                .OrderBy(x => x.CreatedDate).ToList();

        }

        public IList<BloodPressure> GetAllBloodPressures()
        {
            //to list to materialize entities
            var allBloodPressures = _healthContext.BloodPressures.ToList();

            //todo why does groupby generate invalid sql
            return allBloodPressures
                .GroupBy(x => x.CreatedDate.Date)
                .Select(x => new BloodPressure
                {
                    CreatedDate = x.Key.Date,
                    Systolic = x.Average(w => w.Systolic),
                    Diastolic = x.Average(w => w.Diastolic)
                })
                .OrderBy(x => x.CreatedDate).ToList();
        }

        public void Upsert(HeartRate heartRate)
        {
            var existingHeartRate = _healthContext.HeartRates.Find(heartRate.CreatedDate);

            if (existingHeartRate == null)
            {
                //  _logger.Log($"WEIGHT : Insert Weight record : {weight.DateTime:yy-MM-dd} , {weight.Kg} Kg , {weight.FatRatioPercentage} % Fat");
                _healthContext.Add(heartRate);
            }
            else
            {
                // _logger.Log($"WEIGHT : Update Weight record : {weight.DateTime:yy-MM-dd} , {weight.Kg} Kg , {weight.FatRatioPercentage} % Fat");
                existingHeartRate.Bpm = heartRate.Bpm;

            }

            _healthContext.SaveChanges();
        }

        public void Upsert(Weight weight)
        {
            var existingWeight = _healthContext.Weights.Find(weight.CreatedDate);

            if (existingWeight == null)
            {
              //  _logger.Log($"WEIGHT : Insert Weight record : {weight.DateTime:yy-MM-dd} , {weight.Kg} Kg , {weight.FatRatioPercentage} % Fat");
                _healthContext.Add(weight);
            }
            else
            {
               // _logger.Log($"WEIGHT : Update Weight record : {weight.DateTime:yy-MM-dd} , {weight.Kg} Kg , {weight.FatRatioPercentage} % Fat");
                existingWeight.Kg = weight.Kg;
                existingWeight.FatRatioPercentage = weight.FatRatioPercentage;
            }

            _healthContext.SaveChanges();
        }



        public void Upsert(AlcoholIntake alcoholIntake)
        {
            var existingAlcoholIntake = _healthContext.AlcoholIntakes.Find(alcoholIntake.CreatedDate);

            if (existingAlcoholIntake == null)
            {
                //  _logger.Log($"WEIGHT : Insert Weight record : {weight.DateTime:yy-MM-dd} , {weight.Kg} Kg , {weight.FatRatioPercentage} % Fat");
                _healthContext.Add(alcoholIntake);
            }
            else
            {
                // _logger.Log($"WEIGHT : Update Weight record : {weight.DateTime:yy-MM-dd} , {weight.Kg} Kg , {weight.FatRatioPercentage} % Fat");
                existingAlcoholIntake.Units = alcoholIntake.Units;
            }

            _healthContext.SaveChanges();
        }

        public void Upsert(BloodPressure bloodPressure)
        {
            var existingBloodPressure = _healthContext.BloodPressures.Find(bloodPressure.CreatedDate);

            if (existingBloodPressure != null)
            {
                existingBloodPressure.Diastolic = bloodPressure.Diastolic;
                existingBloodPressure.Systolic = bloodPressure.Systolic;

                // _logger.Log($"BLOOD PRESSURE : Updating record : {bloodPressure.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {bloodPressure.Diastolic} mmHg Diastolic , {bloodPressure.Systolic} mmHg Systolic");
                
            }
            else
            {
                //  _logger.Log($"BLOOD PRESSURE : Inserting record : {bloodPressure.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {bloodPressure.Diastolic} mmHg Diastolic , {bloodPressure.Systolic} mmHg Systolic");
                _healthContext.Add(bloodPressure);
            }

            _healthContext.SaveChanges();

        }


        public void Upsert(StepCount stepCount)
        {

            var existingStepCount = _healthContext.StepCounts.Find(stepCount.CreatedDate);
            if (existingStepCount != null)
            {
                // _logger.Log($"STEP COUNT : Update Step Data for {stepCount.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} : {stepCount.Count} steps");
                existingStepCount.Count = stepCount.Count;
                existingStepCount.CumSumCount = stepCount.CumSumCount;
            }
            else
            {
                // _logger.Log($"STEP COUNT : Insert Step Data for {stepCount.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} : {stepCount.Count} steps");
                _healthContext.Add(stepCount);
            }

            _healthContext.SaveChanges();
        }

        public void Upsert(ActivitySummary activitySummary)
        {

            var existingActivitySummary = _healthContext.ActivitySummaries.Find(activitySummary.CreatedDate);
            if (existingActivitySummary == null)
            {
                // _logger.Log($"ACTIVITY SUMMARY : Update Activity Data for {activitySummary.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} : {activitySummary.SedentaryMinutes} sedentary minutes, {activitySummary.LightlyActiveMinutes} lightly active minutes, {activitySummary.FairlyActiveMinutes} fairly active minutes, {activitySummary.VeryActiveMinutes} very active minutes.");
                _healthContext.Add(activitySummary);
            }
            else
            {

                existingActivitySummary.SedentaryMinutes = activitySummary.SedentaryMinutes;
                existingActivitySummary.LightlyActiveMinutes = activitySummary.LightlyActiveMinutes;
                existingActivitySummary.FairlyActiveMinutes = activitySummary.FairlyActiveMinutes;
                existingActivitySummary.VeryActiveMinutes = activitySummary.VeryActiveMinutes;

                existingActivitySummary.CumSumActiveMinutes = activitySummary.CumSumActiveMinutes;
                //_logger.Log($"ACTIVITY SUMMARY : Insert Activity Data for {activitySummary.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} : {activitySummary.SedentaryMinutes} sedentary minutes, {activitySummary.LightlyActiveMinutes} lightly active minutes, {activitySummary.FairlyActiveMinutes} fairly active minutes, {activitySummary.VeryActiveMinutes} very active minutes.");
               
            }


            _healthContext.SaveChanges();
        }

        public void Upsert(RestingHeartRate restingHeartRate)
        {
            var existingRestingHeartRate = _healthContext.RestingHeartRates.Find(restingHeartRate.CreatedDate);

            if (existingRestingHeartRate != null)
            {
                existingRestingHeartRate.Beats = restingHeartRate.Beats;
                existingRestingHeartRate.MovingAverageBeats = restingHeartRate.MovingAverageBeats;
             //   _logger.Log($"RESTING HEART RATE : About to update Resting Heart Rate record : {restingHeartRate.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {restingHeartRate.Beats} beats");
                //_healthRepository.Update(existingRestingHeartRate, restingHeartRate);
            }
            else
            {
                //   _logger.Log($"RESTING HEART RATE : About to insert Resting Heart Rate record : {restingHeartRate.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , {restingHeartRate.Beats} beats");
                _healthContext.Add(restingHeartRate);
            }

            _healthContext.SaveChanges();
        }

        public void Upsert(HeartRateSummary heartSummary)
        {

            var existingHeartSummary = _healthContext.HeartRateSummaries.Find(heartSummary.CreatedDate);

            if (existingHeartSummary != null)
            {

                existingHeartSummary.OutOfRangeMinutes = heartSummary.OutOfRangeMinutes;
                existingHeartSummary.FatBurnMinutes = heartSummary.FatBurnMinutes;
                existingHeartSummary.CardioMinutes = heartSummary.CardioMinutes;
                existingHeartSummary.PeakMinutes = heartSummary.PeakMinutes;

                existingHeartSummary.CumSumCardioAndAbove = heartSummary.CumSumCardioAndAbove;
//                _logger.Log($"HEART SUMMARY : About to update Heart SUmmary Record : {heartSummary.DateTime:dd-MMM-yyyy HH:mm:ss (ddd)} , ");
  
            }
            else
            {
                //  _logger.Log("HEART SUMMARY : insert thing");
                _healthContext.Add(heartSummary);
            }

            _healthContext.SaveChanges();
        }

        public void Upsert(Run run)
        {
            var existingRun = _healthContext.Runs.Find(run.CreatedDate);

            if (existingRun != null)
            {

                existingRun.Metres = run.Metres;
                existingRun.Time = run.Time;
            }
            else
            {
                //  _logger.Log("HEART SUMMARY : insert thing");
                _healthContext.Add(run);
            }

            _healthContext.SaveChanges();
        }

        public void Upsert(Ergo ergo)
        {
            var existingErgo = _healthContext.Ergos.Find(ergo.CreatedDate);

            if (existingErgo == null)
            {
                //  _logger.Log($"WEIGHT : Insert Weight record : {weight.DateTime:yy-MM-dd} , {weight.Kg} Kg , {weight.FatRatioPercentage} % Fat");
                _healthContext.Add(ergo);
            }
            else
            {
                // _logger.Log($"WEIGHT : Update Weight record : {weight.DateTime:yy-MM-dd} , {weight.Kg} Kg , {weight.FatRatioPercentage} % Fat");
                existingErgo.Metres = ergo.Metres;
                existingErgo.Time = ergo.Time;
            }

            _healthContext.SaveChanges();
        }

    }
}