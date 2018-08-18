using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using Utils;

namespace Repositories
{
    public class HealthContext : DbContext
    {
        private  IConfig _config;
        public virtual DbSet<Models.BloodPressure> BloodPressures { get; set; }
        public virtual DbSet<Models.ActivitySummary> ActivitySummaries { get; set; }
        public virtual DbSet<Models.StepCount> StepCounts { get; set; }
        public virtual DbSet<Models.HeartRateSummary> HeartRateSummaries { get; set; }
        public virtual DbSet<Models.RestingHeartRate> RestingHeartRates { get; set; }
        public virtual DbSet<Models.AlcoholIntake> AlcoholIntakes { get; set; }
        public virtual DbSet<Models.Weight> Weights { get; set; }
        public virtual DbSet<Models.Run> Runs { get; set; }
        public virtual DbSet<Models.Ergo> Ergos { get; set; }
        public virtual DbSet<Models.Token> Tokens { get; set; }

        //  public virtual DbSet<Models.HeartRate> HeartRates { get; set; }

        //public HealthContext(IConfig config)
        //{
        //    _config = config;
        //}

        public HealthContext(DbContextOptions<HealthContext> options) : base(options)
        {

        }

        public void EnsureSeedData()
        {
            this.Weights.Add(new Repositories.Models.Weight { CreatedDate = new DateTime(2017, 1, 1), Kg = 123 });

            this.StepCounts.AddRange(new List<StepCount> {
                new StepCount{ CreatedDate = new DateTime(2018,1,1), Count = 1111},
                new StepCount{ CreatedDate = new DateTime(2018,1,2), Count = 2222},
                new StepCount{ CreatedDate = new DateTime(2018,1,3), Count = 3333},

            });

            this.SaveChanges();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        _config = new Config();
        //        var connectionString = _config.HealthDbConnectionString;
        //        optionsBuilder.UseSqlServer(
        //            connectionString,
        //            sqlServerOptionsAction: sqlOptions =>
        //            {
        //                sqlOptions.EnableRetryOnFailure(
        //                    maxRetryCount: 5,
        //                    maxRetryDelay: TimeSpan.FromSeconds(30),
        //                    errorNumbersToAdd: null);
        //            }
        //        );
        //    }
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
        //    }
        //}

    }
}
