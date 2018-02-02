using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HealthAPI.Models
{
    public partial class HealthContext : DbContext
    {
        public virtual DbSet<BloodPressures> BloodPressures { get; set; }
        public virtual DbSet<DailyActivitySummaries> DailyActivitySummaries { get; set; }
        public virtual DbSet<DailySteps> DailySteps { get; set; }
        public virtual DbSet<HeartRateDailySummaries> HeartRateDailySummaries { get; set; }
        public virtual DbSet<RestingHeartRate> RestingHeartRate { get; set; }
        //public virtual DbSet<Salaries> Salaries { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<Weights> Weights { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=tcp:musgrosoft.database.windows.net,1433;Initial Catalog=musgrosoft;Persist Security Info=False;User ID=timmusgrove;Password=Starbucks1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BloodPressures>(entity =>
            {
                entity.HasKey(e => new { e.DateTime, e.DataSource });

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.DataSource)
                    .HasMaxLength(100)
                    .IsUnicode(false);
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

            modelBuilder.Entity<Weights>(entity =>
            {
                entity.HasKey(e => new { e.DateTime, e.DataSource });

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.DataSource)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FatRatioPercentage).HasColumnType("decimal(10, 5)");

                entity.Property(e => e.WeightKg).HasColumnType("decimal(10, 5)");
            });
        }
    }
}
