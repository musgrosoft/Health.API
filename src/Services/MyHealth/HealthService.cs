using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Repositories;
using Repositories.Models;
using Utils;

namespace Services.MyHealth
{
    public class HealthService
    {   
        private readonly IConfig _config;
        private readonly ILogger _logger;
        private readonly HealthContext _healthContext;

        private DateTime MIN_WEIGHT_DATE = new DateTime(2012, 1, 1);
        private DateTime MIN_BLOOD_PRESSURE_DATE = new DateTime(2012, 1, 1);

        private DateTime MIN_FITBIT_DATE = new DateTime(2017, 5, 1);

        public HealthService(IConfig config, ILogger logger, HealthContext healthContext)
        {
            _config = config;
            _logger = logger;
            _healthContext = healthContext;
        }

        public DateTime GetLatestWeightDate()
        {
            var latestWeightDate = _healthContext.Weights.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
            return latestWeightDate ?? MIN_WEIGHT_DATE;
        }

        public DateTime GetLatestBloodPressureDate()
        {
            var latestBloodPressureDate = _healthContext.BloodPressures.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
            return latestBloodPressureDate ?? MIN_BLOOD_PRESSURE_DATE;
        }
        
        public DateTime GetLatestStepCountDate()
        {
            var latestDate = _healthContext.StepCounts.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
            return latestDate ?? MIN_FITBIT_DATE;
        }
        
        public DateTime GetLatestDailyActivityDate()
        {
            var latestDate = _healthContext.DailyActivitySummaries.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
            return latestDate ?? MIN_FITBIT_DATE;
        }

        public DateTime GetLatestRestingHeartRateDate()
        {
            var latestDate = _healthContext.RestingHeartRates.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
            return latestDate ?? MIN_FITBIT_DATE;
        }

        public DateTime GetLatestHeartRateDailySummaryDate()
        {
            var latestDate = _healthContext.HeartRateDailySummaries.OrderByDescending(x => x.DateTime).FirstOrDefault()?.DateTime;
            return latestDate ?? MIN_FITBIT_DATE;
        }
        
        public async Task UpsertWeight(Weight weight)
        {
            var existingWeight = await _healthContext.Weights.FindAsync(weight.DateTime);
            if (existingWeight != null)
            {
                existingWeight.Kg = weight.Kg;
                existingWeight.FatRatioPercentage = weight.FatRatioPercentage;
            }
            else
            {
                await _healthContext.AddAsync(weight);
            }
            
            await _healthContext.SaveChangesAsync();
        }

        public async Task UpsertBloodPressure(BloodPressure bloodPressure)
        {
            var existingBloodPressure = await _healthContext.BloodPressures.FindAsync(bloodPressure.DateTime);
            if (existingBloodPressure != null)
            {
                existingBloodPressure.Diastolic = bloodPressure.Diastolic;
                existingBloodPressure.Systolic = bloodPressure.Systolic;
            }
            else
            {
                await _healthContext.BloodPressures.AddAsync(bloodPressure);
            }

            await _healthContext.SaveChangesAsync();
        }
        
        public async Task UpsertStepCount(StepCount stepCount)
        {
            var existingStepCount = await _healthContext.StepCounts.FindAsync(stepCount.DateTime);
            if (existingStepCount != null)
            {
                existingStepCount.Count = stepCount.Count;
            }
            else
            {
                await _healthContext.StepCounts.AddAsync(stepCount);
            }

            await _healthContext.SaveChangesAsync();
        }
        
        public async Task UpsertDailyActivity(DailyActivity dailyActivity)
        {
            var existingDailyActivity = await _healthContext.DailyActivitySummaries.FindAsync(dailyActivity.DateTime);
            if (existingDailyActivity != null)
            {
                existingDailyActivity.SedentaryMinutes = dailyActivity.SedentaryMinutes;
                existingDailyActivity.LightlyActiveMinutes = dailyActivity.LightlyActiveMinutes;
                existingDailyActivity.FairlyActiveMinutes = dailyActivity.FairlyActiveMinutes;
                existingDailyActivity.VeryActiveMinutes = dailyActivity.VeryActiveMinutes;
            }
            else
            {
                await _healthContext.DailyActivitySummaries.AddAsync(dailyActivity);
            }

            await _healthContext.SaveChangesAsync();
        }

        public async Task UpsertRestingHeartRate(RestingHeartRate restingHeartRate)
        {
            var existingRestingHeartRate = await _healthContext.RestingHeartRates.FindAsync(restingHeartRate.DateTime);
            if (existingRestingHeartRate != null)
            {
                existingRestingHeartRate.Beats = restingHeartRate.Beats;
            }
            else
            {
                await _healthContext.RestingHeartRates.AddAsync(restingHeartRate);
            }

            await _healthContext.SaveChangesAsync();
        }
        
        public async Task UpsertDailyHeartSummary(HeartRateZoneSummary dailyHeartZoneSummary)
        {
            var existingHeartRateZoneSummary = await _healthContext.HeartRateDailySummaries.FindAsync(dailyHeartZoneSummary.DateTime);
            if (existingHeartRateZoneSummary != null)
            {
                existingHeartRateZoneSummary.OutOfRangeMinutes = dailyHeartZoneSummary.OutOfRangeMinutes;
                existingHeartRateZoneSummary.FatBurnMinutes = dailyHeartZoneSummary.FatBurnMinutes;
                existingHeartRateZoneSummary.CardioMinutes = dailyHeartZoneSummary.CardioMinutes;
                existingHeartRateZoneSummary.PeakMinutes = dailyHeartZoneSummary.PeakMinutes;

            }
            else
            {
                await _healthContext.HeartRateDailySummaries.AddAsync(dailyHeartZoneSummary);
            }

            await _healthContext.SaveChangesAsync();
        }

        public async Task AddMovingAveragesToWeights(int period = 10)
        {
            var orderedWeights = _healthContext.Weights.OrderBy(x => x.DateTime).ToList();

            for (int i = 0; i < orderedWeights.Count(); i++)
            {
                if (i >= period - 1)
                {
                    decimal total = 0;
                    for (int x = i; x > (i - period); x--)
                        total += orderedWeights[x].Kg;
                    decimal average = total / period;
                    // result.Add(series.Keys[i], average);
                    orderedWeights[i].MovingAverageKg = average;
                }
                else
                {
                    //weights[i].MovingAverageKg = weights[i].Kg;
                    orderedWeights[i].MovingAverageKg = null;
                }

            }

            await _healthContext.SaveChangesAsync();
        }


        public async Task AddMovingAveragesToBloodPressures(int period = 10)
        {
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
                    // result.Add(series.Keys[i], average);
                    bloodPressures[i].MovingAverageSystolic = averageSystolic;
                    bloodPressures[i].MovingAverageDiastolic = averageDiastolic;
                }
                else
                {
                    //bloodPressures[i].MovingAverageSystolic = bloodPressures[i].Systolic;
                    //bloodPressures[i].MovingAverageDiastolic = bloodPressures[i].Diastolic;
                    bloodPressures[i].MovingAverageSystolic = null;
                    bloodPressures[i].MovingAverageDiastolic = null;
                }


            }

            await _healthContext.SaveChangesAsync();
        }





    }
}