﻿using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.Health.Models;

namespace Repositories.Health
{
    public class HealthRepository : IHealthRepository
    {
        private readonly HealthContext _healthContext;

        public HealthRepository(HealthContext healthContext)
        {
            _healthContext = healthContext;
        }
        
        
        public DateTime? GetLatestBloodPressureDate()
        {
            return _healthContext.BloodPressures.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }

        public DateTime? GetLatestWeightDate()
        {
           return _healthContext.Weights.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }

        public DateTime? GetLatestWithingsSleepDate()
        {
            return _healthContext.MyWithingsSleeps.OrderByDescending(x => x.StartDate).FirstOrDefault()?.StartDate;
        }

        public Weight GetLatestWeight()
        {
            return _healthContext.Weights.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        }

        public List<BloodPressure> GetLatestBloodPressures(int num)
        {
            return  _healthContext.BloodPressures.OrderByDescending(x => x.CreatedDate).Take(num).ToList();
        }

        public DateTime? GetLatestExerciseDate()
        {
            return _healthContext.Exercises.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }

        public List<Exercise> GetLatestExercises(int num)
        {
            return _healthContext.Exercises.OrderByDescending(x => x.CreatedDate).Take(num).ToList();
        }

//        public Exercise GetFastest15MinuteErgo(DateTime fromDate)
//        {
//            return _healthContext.Exercises
//                .Where(x => x.Description.ToLower() == "ergo")
//                .Where(x => x.TotalSeconds == 900)
//                .Where(x => x.CreatedDate >= fromDate)
//                .OrderByDescending(x => x.Metres)
//                .FirstOrDefault();
//        }

        public Exercise GetFurthest(DateTime fromDate, string exerciseType, int totalSeconds)
        {
            return _healthContext.Exercises
                .Where(x => x.Description.ToLower() == exerciseType.ToLower())
                .Where(x => x.TotalSeconds == totalSeconds)
                .Where(x => x.CreatedDate >= fromDate)
                .OrderByDescending(x => x.Metres)
                .FirstOrDefault();
        }

        public List<Drink> GetLatestDrinks(int num)
        {
            return _healthContext.Drinks.OrderByDescending(x => x.CreatedDate).Take(num).ToList();
        }

        public List<RestingHeartRate> GetLatestRestingHeartRate(int num)
        {
            return _healthContext.RestingHeartRates.OrderByDescending(x => x.CreatedDate).Take(num).ToList();
        }

        public double GetCumSumUnits()
        {
            return _healthContext.Drinks.Sum(x=>x.Units);
        }

        public double GetCumSumCardioMinutes()
        {
            return (_healthContext.Exercises.Sum(x => x.TotalSeconds))/60;
        }



        public Target GetTarget(DateTime date)
        {
            return _healthContext.Targets.First(x => x.Date.Date == date.Date);
        }

        public List<BloodPressure> GetLatestBloodPressure(int num)
        {
            return _healthContext.BloodPressures.OrderByDescending(x => x.CreatedDate).Take(num).ToList();
        }



        public List<Weight> GetLatestWeights(int num)
        {
            return _healthContext.Weights.OrderByDescending(x => x.CreatedDate).Take(num).ToList();
        }

        public DateTime? GetLatestRestingHeartRateDate()
        {
            return _healthContext.RestingHeartRates.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }

        public DateTime? GetLatestDrinkDate()
        {
            return _healthContext.Drinks.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }

        public DateTime? GetLatestFitbitSleepDate()
        {
            return _healthContext.MyFitbitSleeps.OrderByDescending(x => x.DateOfSleep).FirstOrDefault()?.DateOfSleep;
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

        public void Upsert(MyFitbitSleep myFitbitSleep)
        {
            var existingSleep = _healthContext.MyFitbitSleeps.Find(myFitbitSleep.LogId);

            if (existingSleep == null)
            {
                _healthContext.Add(myFitbitSleep);
            }
            else
            {
                existingSleep.AwakeCount = myFitbitSleep.AwakeCount;
                existingSleep.AwakeDuration = myFitbitSleep.AwakeDuration;
                existingSleep.AwakeningsCount = myFitbitSleep.AwakeningsCount;
                existingSleep.DateOfSleep = myFitbitSleep.DateOfSleep;
                existingSleep.Duration = myFitbitSleep.Duration;
                existingSleep.Efficiency = myFitbitSleep.Efficiency;
                existingSleep.EndTime = myFitbitSleep.EndTime;
                existingSleep.MinutesAfterWakeup = myFitbitSleep.MinutesAfterWakeup;
                existingSleep.MinutesAsleep = myFitbitSleep.MinutesAsleep;
                existingSleep.MinutesAwake = myFitbitSleep.MinutesAwake;
                existingSleep.MinutesToFallAsleep = myFitbitSleep.MinutesToFallAsleep;
                existingSleep.RestlessCount = myFitbitSleep.RestlessCount;
                existingSleep.RestlessDuration = myFitbitSleep.RestlessDuration;
                existingSleep.StartTime = myFitbitSleep.StartTime;
                existingSleep.TimeInBed = myFitbitSleep.TimeInBed;
            }

            _healthContext.SaveChanges();
        }

        public void Upsert(MyWithingsSleep myWithingsSleep)
        {
            var existingSleep = _healthContext.MyWithingsSleeps.Find(myWithingsSleep.Id);

            if (existingSleep == null)
            {
                _healthContext.Add(myWithingsSleep);
            }
            else
            {
                existingSleep.StartDate = myWithingsSleep.StartDate;
                existingSleep.DeepSleepDuration = myWithingsSleep.DeepSleepDuration;
                existingSleep.DurationToSleep = myWithingsSleep.DurationToSleep;
                existingSleep.DurationToWakeUp = myWithingsSleep.DurationToWakeUp;
                existingSleep.EndDate = myWithingsSleep.EndDate;
                existingSleep.HeartRateAvg = myWithingsSleep.HeartRateAvg;
                existingSleep.HeartRateMax = myWithingsSleep.HeartRateMax;
                existingSleep.HeartRateMin = myWithingsSleep.HeartRateMin;
                existingSleep.LightSleepDuration = myWithingsSleep.LightSleepDuration;
                existingSleep.ModifiedDate = myWithingsSleep.ModifiedDate;
                existingSleep.RemSleepDuration = myWithingsSleep.RemSleepDuration;
                existingSleep.RespirationRateAvg = myWithingsSleep.RespirationRateAvg;
                existingSleep.RespirationRateMax = myWithingsSleep.RespirationRateMax;
                existingSleep.RespirationRateMin = myWithingsSleep.RespirationRateMin;
                existingSleep.TimeZone = myWithingsSleep.TimeZone;
                existingSleep.WakeUpCount = myWithingsSleep.WakeUpCount;
                existingSleep.WakeUpDuration = myWithingsSleep.WakeUpDuration;

            }

            _healthContext.SaveChanges();
        }


        public void Upsert(Drink drink)
        {
            var existingAlcoholIntake = _healthContext.Drinks.Find(drink.CreatedDate);

            if (existingAlcoholIntake == null)
            {
                //  _logger.Log($"WEIGHT : Insert Weight record : {weight.DateTime:yy-MM-dd} , {weight.Kg} Kg , {weight.FatRatioPercentage} % Fat");
                _healthContext.Add(drink);
            }
            else
            {
                // _logger.Log($"WEIGHT : Update Weight record : {weight.DateTime:yy-MM-dd} , {weight.Kg} Kg , {weight.FatRatioPercentage} % Fat");
                existingAlcoholIntake.Units = drink.Units;
            }

            _healthContext.SaveChanges();
        }

        public void Upsert(Exercise exercise)
        {
            var existingExercise = _healthContext.Exercises.Find(exercise.CreatedDate, exercise.Description);

            if (existingExercise == null)
            {
                _healthContext.Add(exercise);
            }
            else
            {
                existingExercise.Metres = exercise.Metres;
                existingExercise.TotalSeconds = exercise.TotalSeconds;
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



        public void Upsert(RestingHeartRate restingHeartRate)
        {
            var existingRestingHeartRate = _healthContext.RestingHeartRates.Find(restingHeartRate.CreatedDate);

            if (existingRestingHeartRate != null)
            {
                existingRestingHeartRate.Beats = restingHeartRate.Beats;
              //  existingRestingHeartRate.MovingAverageBeats = restingHeartRate.MovingAverageBeats;
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




    }
}