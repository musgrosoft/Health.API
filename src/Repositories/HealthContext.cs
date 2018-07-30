using System;
using Microsoft.EntityFrameworkCore;
using Utils;

namespace Repositories
{
    public class HealthContext : DbContext
    {
        private readonly IConfig _config;
        public virtual DbSet<Models.BloodPressure> BloodPressures { get; set; }
        public virtual DbSet<Models.ActivitySummary> ActivitySummaries { get; set; }
        public virtual DbSet<Models.StepCount> StepCounts { get; set; }
        public virtual DbSet<Models.HeartRateSummary> HeartRateSummaries { get; set; }
        public virtual DbSet<Models.RestingHeartRate> RestingHeartRates { get; set; }
        public virtual DbSet<Models.AlcoholIntake> AlcoholIntakes { get; set; }
        public virtual DbSet<Models.Weight> Weights { get; set; }
        public virtual DbSet<Models.Run> Runs { get; set; }
        public virtual DbSet<Models.Ergo> Ergos { get; set; }

      //  public virtual DbSet<Models.HeartRate> HeartRates { get; set; }

        public HealthContext(IConfig config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _config.HealthDbConnectionString;
                optionsBuilder.UseSqlServer(
                    connectionString, 
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    }
                );
            }
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
        //    }
        //}

    }
}
