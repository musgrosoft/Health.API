using System;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;
using Utils;

namespace Repositories.Health
{
    public interface IHealthRepository
    {

        void Insert<T>(T obj) where T : class;
        void Update(Weight existingWeight, Weight newWeight);

        DateTime? GetLatestStepCountDate();
        DateTime? GetLatestBloodPressureDate();
        DateTime? GetLatestWeightDate();
        DateTime? GetLatestActivitySummaryDate();
        DateTime? GetLatestRestingHeartRateDate();
        DateTime? GetLatestHeartSummaryDate();

        Weight Find(Weight weight);
        BloodPressure Find(BloodPressure bloodPressure);
        StepCount Find(StepCount stepCount);
        ActivitySummary Find(ActivitySummary activitySummary);
        RestingHeartRate Find(RestingHeartRate restingHeartRate);
        HeartSummary Find(HeartSummary heartSummary);
        void Update(BloodPressure existingBloodPressure, BloodPressure bloodPressure);
        void Update(StepCount existingStepCount, StepCount stepCount);
        void Update(ActivitySummary existingDailyActivity, ActivitySummary dailyActivity);
        void Update(RestingHeartRate existingRestingHeartRate, RestingHeartRate restingHeartRate);
        void Update(HeartSummary existingHeartSummary, HeartSummary heartSummary);
    }

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

        public void Update(ActivitySummary existingDailyActivity, ActivitySummary dailyActivity)
        {
            existingDailyActivity.SedentaryMinutes = dailyActivity.SedentaryMinutes;
            existingDailyActivity.LightlyActiveMinutes = dailyActivity.LightlyActiveMinutes;
            existingDailyActivity.FairlyActiveMinutes = dailyActivity.FairlyActiveMinutes;
            existingDailyActivity.VeryActiveMinutes = dailyActivity.VeryActiveMinutes;

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