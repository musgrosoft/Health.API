using System;
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

        List<Exercise> GetLatestExercises(int num)
        {
            return _healthContext.Exercises.OrderByDescending(x => x.CreatedDate).Take(num).ToList();
        }

        List<Exercise> GetLatest15MinuteErgos(int num)
        {
            return _healthContext.Exercises
                .Where(x => x.Description.ToLower() == "ergo")
                .Where(x => x.TotalSeconds == 900)
                .OrderByDescending(x => x.CreatedDate)
                .Take(num)
                .ToList();
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