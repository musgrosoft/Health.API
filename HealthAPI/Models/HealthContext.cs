using System;
using Microsoft.EntityFrameworkCore;

namespace HealthAPI.Models
{

    public class HealthContext : DbContext
    {
        public virtual DbSet<BloodPressure> BloodPressures { get; set; }
        public virtual DbSet<DailyActivitySummaries> DailyActivitySummaries { get; set; }
        public virtual DbSet<DailySteps> DailySteps { get; set; }
        public virtual DbSet<HeartRateDailySummaries> HeartRateDailySummaries { get; set; }
        public virtual DbSet<RestingHeartRate> RestingHeartRate { get; set; }
        //public virtual DbSet<Salaries> Salaries { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<Weight> Weights { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = Environment.GetEnvironmentVariable("HealthDbConnectionString");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<BloodPressure>(entity =>
            {
                entity.HasKey(e => new { e.DateTime});

                entity.Property(e => e.DateTime).HasColumnType("datetime");
                
            });

            modelBuilder.Entity<DailyActivitySummaries>(entity =>
            {
                entity.HasKey(e => new { e.DateTime, e.DataSource });

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.DataSource)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DailySteps>(entity =>
            {
                entity.HasKey(e => new { e.DateTime, e.DataSource });

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.DataSource)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<HeartRateDailySummaries>(entity =>
            {
                entity.HasKey(e => new { e.DateTime, e.DataSource });

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.DataSource)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RestingHeartRate>(entity =>
            {
                entity.HasKey(e => new { e.DateTime, e.DataSource });

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.DataSource)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            //modelBuilder.Entity<Salaries>(entity =>
            //{
            //    entity.HasKey(e => new { e.Keyword, e.Location });

            //    entity.Property(e => e.Keyword)
            //        .HasMaxLength(200)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Location)
            //        .HasMaxLength(200)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Average)
            //        .HasMaxLength(200)
            //        .IsUnicode(false);

            //    entity.Property(e => e.High)
            //        .HasMaxLength(200)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Low)
            //        .HasMaxLength(200)
            //        .IsUnicode(false);
            //});

            modelBuilder.Entity<Units>(entity =>
            {
                entity.HasKey(e => e.DateTime);

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.Units1).HasColumnName("Units");
            });

            modelBuilder.Entity<Weight>(entity =>
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
