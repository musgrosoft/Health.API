using Microsoft.EntityFrameworkCore;
using Repositories.Health.Models;
using Repositories.OAuth.Models;

namespace Repositories
{
    public class HealthContext : DbContext
    {
        public virtual DbSet<BloodPressure> BloodPressures { get; set; }
        public virtual DbSet<ActivitySummary> ActivitySummaries { get; set; }
        public virtual DbSet<StepCount> StepCounts { get; set; }
        public virtual DbSet<HeartRateSummary> HeartRateSummaries { get; set; }
        public virtual DbSet<RestingHeartRate> RestingHeartRates { get; set; }
        public virtual DbSet<AlcoholIntake> AlcoholIntakes { get; set; }
        public virtual DbSet<Weight> Weights { get; set; }
        public virtual DbSet<Run> Runs { get; set; }
        public virtual DbSet<Ergo> Ergos { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<HeartRate> HeartRates { get; set; }

        public HealthContext(DbContextOptions<HealthContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HeartRateSummary>().HasKey(c =>  new {c.CreatedDate, c.Source});
            modelBuilder.Entity<HeartRate>().HasKey(c => new { c.CreatedDate, c.Source });
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
