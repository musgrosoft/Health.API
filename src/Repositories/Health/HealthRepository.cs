using System;
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

        public void Insert<T>(T obj) where T : class
        {
            _healthContext.Add(obj);
            _healthContext.SaveChanges();
        }
        
        public DateTime? GetLatestStepCountDate()
        {
            return _healthContext.StepCounts.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
        }

        public DateTime? GetLatestBloodPressureDate()
        {
            return _healthContext.BloodPressures.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
        }

        public DateTime? GetLatestWeightDate()
        {
           return _healthContext.Weights.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
        }

        public DateTime? GetLatestAlcoholIntakeDate()
        {
            return _healthContext.AlcoholIntakes.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
        }

        public DateTime? GetLatestActivitySummaryDate()
        {
            return _healthContext.ActivitySummaries.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
        }

        public DateTime? GetLatestRestingHeartRateDate()
        {
            return _healthContext.RestingHeartRates.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
        }

        public DateTime? GetLatestHeartSummaryDate()
        {
            return _healthContext.HeartSummaries.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
        }

        public DateTime? GetLatestRunDate()
        {
            return _healthContext.Runs.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
        }

        public Weight Find(Weight weight)
        {
            return _healthContext.Weights.Find(weight.DateTime);
        }

        public BloodPressure Find(BloodPressure bloodPressure)
        {
            return _healthContext.BloodPressures.Find(bloodPressure.DateTime);
        }

        public StepCount Find(StepCount stepCount)
        {
            return _healthContext.StepCounts.Find(stepCount.DateTime);
        }

        public ActivitySummary Find(ActivitySummary activitySummary)
        {
            return _healthContext.ActivitySummaries.Find(activitySummary.DateTime);
        }

        public RestingHeartRate Find(RestingHeartRate restingHeartRate)
        {
            return _healthContext.RestingHeartRates.Find(restingHeartRate.DateTime);
        }

        public HeartSummary Find(HeartSummary heartSummary)
        {
            return _healthContext.HeartSummaries.Find(heartSummary.DateTime);
        }

        public void Update(Weight existingWeight, Weight newWeight)
        {
            //check if datetimes are equal ???
            if (existingWeight.DateTime != newWeight.DateTime)
            {
                throw new Exception("DateTimes not equal on existing and new weights.");
            }

            existingWeight.Kg = newWeight.Kg;
            existingWeight.FatRatioPercentage = newWeight.FatRatioPercentage;

            _healthContext.SaveChanges();
        }

        public void Update(BloodPressure existingBloodPressure, BloodPressure bloodPressure)
        {
            existingBloodPressure.Diastolic = bloodPressure.Diastolic;
            existingBloodPressure.Systolic = bloodPressure.Systolic;

            _healthContext.SaveChanges();

        }

        public void Update(StepCount existingStepCount, StepCount stepCount)
        {
            existingStepCount.Count = stepCount.Count;
            _healthContext.SaveChanges();
        }

        public void Update(ActivitySummary existingActivitySummary, ActivitySummary activitySummary)
        {
            existingActivitySummary.SedentaryMinutes = activitySummary.SedentaryMinutes;
            existingActivitySummary.LightlyActiveMinutes = activitySummary.LightlyActiveMinutes;
            existingActivitySummary.FairlyActiveMinutes = activitySummary.FairlyActiveMinutes;
            existingActivitySummary.VeryActiveMinutes = activitySummary.VeryActiveMinutes;

            _healthContext.SaveChanges();
        }

        public void Update(RestingHeartRate existingRestingHeartRate, RestingHeartRate restingHeartRate)
        {
            existingRestingHeartRate.Beats = restingHeartRate.Beats;

            _healthContext.SaveChanges();
        }

        public void Update(HeartSummary existingHeartSummary, HeartSummary heartSummary)
        {
            existingHeartSummary.OutOfRangeMinutes = heartSummary.OutOfRangeMinutes;
            existingHeartSummary.FatBurnMinutes = heartSummary.FatBurnMinutes;
            existingHeartSummary.CardioMinutes = heartSummary.CardioMinutes;
            existingHeartSummary.PeakMinutes = heartSummary.PeakMinutes;

            _healthContext.SaveChanges();
        }


    }
}