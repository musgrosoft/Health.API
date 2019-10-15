using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Repositories.Health.Models;
using Repositories.OAuth.Models;
using Utils;

namespace Repositories
{
    public class HealthContext : DbContext
    {
        public virtual DbSet<BloodPressure> BloodPressures { get; set; }
        public virtual DbSet<RestingHeartRate> RestingHeartRates { get; set; }
        public virtual DbSet<Drink> Drinks { get; set; }
        public virtual DbSet<Weight> Weights { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<Target> Targets { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<CalendarDate> CalendarDates { get; set; }
        public virtual DbSet<SleepSummary> SleepSummaries { get; set; }
        public virtual DbSet<SleepState> SleepStates { get; set; }

        private readonly ILogger _logger;
        
        public HealthContext(DbContextOptions<HealthContext> options, ILogger logger) : base(options)
        {
            _logger = logger;
        }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Exercise>().HasKey(c => new { c.CreatedDate, c.Description });

            if (this.Database.IsSqlServer())
            {

                //Seed CalendarDates
                for (var date = new DateTime(2010, 1, 1); date < new DateTime(2030, 1, 1); date = date.AddDays(1))
                {
                    modelBuilder.Entity<CalendarDate>().HasData(new CalendarDate {Date = date});
                }

                //Seed Weights
                var weightsJson = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Seed Data/Weights.json");
                var weights = JsonConvert.DeserializeObject<List<Weight>>(weightsJson);
                foreach (var weight in weights)
                {
                    modelBuilder.Entity<Weight>().HasData(weight);
                }

                //Seed Blood Pressures
                var bloodPressuresJson = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Seed Data/BloodPressures.json");
                var bloodPressures = JsonConvert.DeserializeObject<List<BloodPressure>>(bloodPressuresJson);
                foreach (var bloodPressure in bloodPressures)
                {
                    modelBuilder.Entity<BloodPressure>().HasData(bloodPressure);
                }

                //Seed Exercises
                var exercisesJson = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Seed Data/Exercises.json");
                var exercises = JsonConvert.DeserializeObject<List<Exercise>>(exercisesJson);
                foreach (var exercise in exercises)
                {
                    modelBuilder.Entity<Exercise>().HasData(exercise);
                }

                //Seed Resting Heart Rates
                var restingHeartRateJson = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Seed Data/RestingHeartRates.json");
                var restingHeartRates = JsonConvert.DeserializeObject<List<RestingHeartRate>>(restingHeartRateJson);
                foreach (var restingHeartRate in restingHeartRates)
                {
                    modelBuilder.Entity<RestingHeartRate>().HasData(restingHeartRate);
                }

                //Seed Units
                var drinksJson = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Seed Data/Drinks.json");
                var drinks = JsonConvert.DeserializeObject<List<Drink>>(drinksJson);
                foreach (var drink in drinks)
                {
                    modelBuilder.Entity<Drink>().HasData(drink);
                }


                for (var date = new DateTime(2018, 5, 1); date < new DateTime(2020, 1, 1); date = date.AddDays(1))
                {
                    modelBuilder.Entity<Target>().HasData(new Target
                    {
                        Date = date,
                        Kg = GetTargetKg(date),
                        MetresErgo15Minutes = GetTargetErgoMetresFor15Minutes(date),
                        MetresTreadmill30Minutes = GetTargetMetresFor30MinuteTreadmill(date),
                        Diastolic = 80,
                        Systolic = 120,
                        Units = 4,
                        CardioMinutes = 11
                    });
                }

            }
        }

        private double GetTargetKg(DateTime date)
        {
            if (date >= new DateTime(2019, 1, 1))
            {
                return 86 - ((3.000 / 365) * (date - new DateTime(2019, 1, 1)).TotalDays);
            }
            else if (date >= new DateTime(2018, 9, 1))
            {
                return 88.7 - ((0.250 / 30) * (date - new DateTime(2018, 9, 1)).TotalDays);
            }
            else if (date >= new DateTime(2018, 5, 1))
            {
                return 90.74 - ((0.500 / 30) * (date - new DateTime(2018, 5, 1)).TotalDays);
            }

            return 100;
        }

        private int GetTargetErgoMetresFor15Minutes(DateTime date)
        {
            return (int)(3386 + (date - new DateTime(2019, 1, 1)).TotalDays);
        }

        private int GetTargetMetresFor30MinuteTreadmill(DateTime date)
        {
            return (int)(5000 + 2.74725275 * (date - new DateTime(2019, 1, 1)).TotalDays);
        }


    }
}
