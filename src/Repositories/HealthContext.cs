using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class HealthContext : DbContext
    {
        public virtual DbSet<Models.BloodPressure> BloodPressures { get; set; }
        public virtual DbSet<Models.ActivitySummary> ActivitySummaries { get; set; }
        public virtual DbSet<Models.StepCount> StepCounts { get; set; }
        public virtual DbSet<Models.HeartSummary> HeartSummaries { get; set; }
        public virtual DbSet<Models.RestingHeartRate> RestingHeartRates { get; set; }
        public virtual DbSet<Models.AlcoholIntake> AlcoholIntakes { get; set; }
        public virtual DbSet<Models.Weight> Weights { get; set; }
        public virtual DbSet<Models.Run> Runs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Server=tcp:musgrosoft.database.windows.net,1433;Initial Catalog=Health;Persist Security Info=False;User ID=timmusgrove;Password=Starbucks1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";// Environment.GetEnvironmentVariable("HealthDbConnectionString");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        
    }
}
