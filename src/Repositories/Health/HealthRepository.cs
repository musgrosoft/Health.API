using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public DateTime? GetLatestTargetDate()
        {
            return _healthContext.Weights.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }

        public DateTime? GetLatestExerciseDate()
        {
            return _healthContext.Exercises.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }
        public DateTime? GetLatestRestingHeartRateDate()
        {
            return _healthContext.RestingHeartRates.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }

        public DateTime? GetLatestDrinkDate()
        {
            return _healthContext.Drinks.OrderByDescending(x => x.CreatedDate).FirstOrDefault()?.CreatedDate;
        }

        public DateTime? GetLatestSleepSummaryDate()
        {
            return _healthContext.SleepSummaries.OrderByDescending(x => x.DateOfSleep).FirstOrDefault()?.DateOfSleep;
        }
        

        public async Task UpsertAsync(IEnumerable<Weight> weights)
        {
            for (int i = 0; i < weights.Count(); i += 500)
            {
                await _healthContext
                    .UpsertRange(weights.Skip(i).Take(500))
                    .RunAsync();

                _healthContext.SaveChanges();
            }
        }

        public async Task UpsertAsync(IEnumerable<SleepSummary> sleepSummaries)
        {
            for (int i = 0; i < sleepSummaries.Count(); i += 250)
            {
                await _healthContext
                    .UpsertRange(sleepSummaries.Skip(i).Take(250))
                    .RunAsync();

                _healthContext.SaveChanges();
            }
        }
        
        public async Task UpsertAsync(IEnumerable<Drink> drinks)
        {
            var enumerable = drinks.ToList();
            for (int i = 0; i < enumerable.Count(); i += 500)
            {
                await _healthContext
                            .UpsertRange(enumerable.Skip(i).Take(500))
                            .RunAsync();

                _healthContext.SaveChanges();
            }
        }

        public async Task UpsertAsync(IEnumerable<Exercise> exercises)
        {
            for (int i = 0; i < exercises.Count(); i += 500)
            {
                await _healthContext
                            .UpsertRange(exercises.Skip(i).Take(500))
                            .RunAsync();

                _healthContext.SaveChanges();
            }
        }

        public async Task UpsertAsync(IEnumerable<BloodPressure> bloodPressures)
        {
            for (int i = 0; i < bloodPressures.Count(); i += 500)
            {
                await _healthContext
                            .UpsertRange(bloodPressures.Skip(i).Take(500))
                            .RunAsync();

                _healthContext.SaveChanges();
            }
        }

        public async Task UpsertAsync(IEnumerable<RestingHeartRate> restingHeartRates)
        {
            for (int i = 0; i < restingHeartRates.Count(); i += 500)
            {
                await _healthContext
                            .UpsertRange(restingHeartRates.Skip(i).Take(500))
                            .RunAsync();

                _healthContext.SaveChanges();
            }
        }


        public async Task UpsertAsync(IEnumerable<Target> targets)
        {
            for (int i = 0; i < targets.Count(); i += 200)
            {
                await _healthContext
                    .UpsertRange(targets.Skip(i).Take(200))
                    .RunAsync();

                _healthContext.SaveChanges();
            }
        }

    }
}