using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;
using Utils;

namespace Repositories.Health
{
    public class HealthRepository : IHealthRepository
    {
        private readonly HealthContext _healthContext;

        public HealthRepository(HealthContext healthContext)
        {
            _healthContext = healthContext;
        }

        //public void Insert<T>(T obj) where T : class
        //{
        //    _healthContext.Add(obj);
        //    _healthContext.SaveChanges();
        //}

        public void Delete<T>(T obj) where T : class
        {
            _healthContext.Remove<T>(obj);
            _healthContext.SaveChanges();
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

        public IList<Weight> GetLatestWeights(int number, DateTime beforeDate)
        {
            return _healthContext.Weights.Where(x => x.CreatedDate < beforeDate).OrderByDescending(x => x.CreatedDate).Take(number).ToList();
        }

        public IList<Weight> GetWeightsFromDate(DateTime afterDate)
        {
            return _healthContext.Weights.Where(x => x.CreatedDate > afterDate).OrderByDescending(x => x.CreatedDate).ToList();
        }


        public IList<HeartRateSummary> GetLatestHeartSummaries(int number, DateTime beforeDate)
        {
            return _healthContext.HeartRateSummaries.Where(x => x.CreatedDate < beforeDate).OrderByDescending(x => x.CreatedDate).Take(number).ToList();
        }

        public IEnumerable<BloodPressure> GetLatestBloodPressures(int number, DateTime beforeDate)
        {
            return _healthContext.BloodPressures.Where(x => x.CreatedDate < beforeDate).OrderByDescending(x => x.CreatedDate).Take(number);
        }

        public IEnumerable<RestingHeartRate> GetLatestRestingHeartRates(int number, DateTime beforeDate)
        {
            return _healthContext.RestingHeartRates.Where(x => x.CreatedDate < beforeDate).OrderByDescending(x => x.CreatedDate).Take(number);
        }

        public IList<StepCount> GetLatestStepCounts(int number, DateTime beforeDate)
        {
            return _healthContext.StepCounts.Where(x=>x.CreatedDate < beforeDate).OrderByDescending(x => x.CreatedDate).Take(number).ToList();
        }

        public IList<ActivitySummary> GetLatestActivitySummaries(int number, DateTime beforeDate)
        {
            return _healthContext.ActivitySummaries.Where(x => x.CreatedDate < beforeDate).OrderByDescending(x => x.CreatedDate).Take(number).ToList();
        }

        public IEnumerable<AlcoholIntake> GetAllAlcoholIntakes()
        {
            return _healthContext.AlcoholIntakes.OrderByDescending(x => x.CreatedDate);
        }

        public IEnumerable<ActivitySummary> GetAllActivitySummaries()
        {
            return _healthContext.ActivitySummaries.OrderByDescending(x => x.CreatedDate);
        }

        public IEnumerable<HeartRateSummary> GetAllHeartRateSummaries()
        {
            return _healthContext.HeartRateSummaries.OrderByDescending(x => x.CreatedDate);
        }

        public IList<Weight> GetAllWeights()
        {

            var allWeights = _healthContext.Weights.ToList();
//            var copy = allWeights.Select(x => new Weight {
//                CreatedDate = x.CreatedDate,
//                Kg = x.Kg
//            });

            return allWeights
                .GroupBy(x => x.CreatedDate.Date)
                .Select(x => new Weight
                {
                    CreatedDate = x.Key.Date,
                    Kg = x.Average(w => w.Kg)
                })
                .OrderBy(x=>x.CreatedDate).ToList();
        }

        public IList<BloodPressure> GetAllBloodPressures()
        {

            var allBloodPressures = _healthContext.BloodPressures.ToList();
            //            var copy = allWeights.Select(x => new Weight {
            //                CreatedDate = x.CreatedDate,
            //                Kg = x.Kg
            //            });

            return allBloodPressures
                .GroupBy(x => x.CreatedDate.Date)
                .Select(x => new BloodPressure
                {
                    CreatedDate = x.Key.Date,
                    Systolic = x.Average(w => w.Systolic)
                })
                .OrderBy(x => x.CreatedDate).ToList();
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
                existingWeight.MovingAverageKg = weight.MovingAverageKg;
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
                existingAlcoholIntake.CumSumUnits = alcoholIntake.CumSumUnits;
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
                existingBloodPressure.MovingAverageDiastolic = bloodPressure.MovingAverageDiastolic;
                existingBloodPressure.MovingAverageSystolic = bloodPressure.MovingAverageSystolic;

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


    }
}