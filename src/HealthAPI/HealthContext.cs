using System;
using Microsoft.EntityFrameworkCore;

namespace HealthAPI
{

    public class HealthContext : DbContext
    {
        public virtual DbSet<Models.BloodPressure> BloodPressures { get; set; }
        public virtual DbSet<Models.DailyActivitySummary> DailyActivitySummaries { get; set; }
        public virtual DbSet<Models.DailySteps> DailySteps { get; set; }
        public virtual DbSet<Models.HeartRateDailySummary> HeartRateDailySummaries { get; set; }
        public virtual DbSet<Models.RestingHeartRate> RestingHeartRates { get; set; }
        public virtual DbSet<Models.Units> Units { get; set; }
        public virtual DbSet<Models.Weight> Weights { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Server=tcp:musgrosoft.database.windows.net,1433;Initial Catalog=Health;Persist Security Info=False;User ID=timmusgrove;Password=Starbucks1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";// Environment.GetEnvironmentVariable("HealthDbConnectionString");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            

            modelBuilder.Entity<Models.BloodPressure>(entity =>
            {
                entity.HasKey(e => new { e.DateTime});

                entity.Property(e => e.DateTime).HasColumnType("datetime");
                
            });

            modelBuilder.Entity<Models.DailyActivitySummary>(entity =>
            {
                entity.HasKey(e => new { e.DateTime });

                entity.Property(e => e.DateTime).HasColumnType("datetime");

            });

            modelBuilder.Entity<Models.DailySteps>(entity =>
            {
                entity.HasKey(e => new { e.DateTime });

                entity.Property(e => e.DateTime).HasColumnType("datetime");

            });

            modelBuilder.Entity<Models.HeartRateDailySummary>(entity =>
            {
                entity.HasKey(e => new { e.DateTime});

                entity.Property(e => e.DateTime).HasColumnType("datetime");

            });

            modelBuilder.Entity<Models.RestingHeartRate>(entity =>
            {
                entity.HasKey(e => new { e.DateTime });

                entity.Property(e => e.DateTime).HasColumnType("datetime");

            });
            
            modelBuilder.Entity<Models.Units>(entity =>
            {
                entity.HasKey(e => e.DateTime);

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.Units1).HasColumnName("Units");
            });

            modelBuilder.Entity<Models.Weight>(entity =>
            {
                entity.HasKey(e => new { e.DateTime});

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                //entity.Property(e => e.DataSource)
                //    .HasMaxLength(100)
                //    .IsUnicode(false);

                entity.Property(e => e.FatRatioPercentage).HasColumnType("decimal(10, 5)");

                entity.Property(e => e.WeightKg).HasColumnType("decimal(10, 5)");
            });
        }
    }
}
