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
        public virtual DbSet<Models.HeartSummary> HeartSummaries { get; set; }
        public virtual DbSet<Models.RestingHeartRate> RestingHeartRates { get; set; }
        public virtual DbSet<Models.AlcoholIntake> AlcoholIntakes { get; set; }
        public virtual DbSet<Models.Weight> Weights { get; set; }
        public virtual DbSet<Models.Run> Runs { get; set; }

        public HealthContext(IConfig config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _config.HealthDbConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
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
