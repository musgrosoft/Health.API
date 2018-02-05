using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HealthAPI.Models
{
    public interface IHealthContext
    {
        DbSet<BloodPressures> BloodPressures { get; set; }
        DbSet<DailyActivitySummaries> DailyActivitySummaries { get; set; }
        DbSet<DailySteps> DailySteps { get; set; }
        DbSet<HeartRateDailySummaries> HeartRateDailySummaries { get; set; }
        DbSet<RestingHeartRate> RestingHeartRate { get; set; }
        DbSet<Units> Units { get; set; }
        DbSet<Weights> Weights { get; set; }
        DatabaseFacade Database { get; }
        ChangeTracker ChangeTracker { get; }
        IModel Model { get; }
   //     DbSet Set() where TEntity : class;
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
        void Dispose();
        //EntityEntry Entry(TEntity entity) where TEntity : class;
        //EntityEntry Entry(object entity);
        //EntityEntry Add(TEntity entity) where TEntity : class;
        //Task AddAsync(TEntity entity, CancellationToken cancellationToken) where TEntity : class;
        //EntityEntry Attach(TEntity entity) where TEntity : class;
        //EntityEntry Update(TEntity entity) where TEntity : class;
        //EntityEntry Remove(TEntity entity) where TEntity : class;
        EntityEntry Add(object entity);
        Task<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken);
        EntityEntry Attach(object entity);
        EntityEntry Update(object entity);
        EntityEntry Remove(object entity);
        void AddRange(params object[] entities);
        Task AddRangeAsync(params object[] entities);
        void AttachRange(params object[] entities);
        void UpdateRange(params object[] entities);
        void RemoveRange(params object[] entities);
        void AddRange(IEnumerable<object> entities);
        Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken);
        void AttachRange(IEnumerable<object> entities);
        void UpdateRange(IEnumerable<object> entities);
        void RemoveRange(IEnumerable<object> entities);
        object Find(Type entityType, params object[] keyValues);
        Task<object> FindAsync(Type entityType, params object[] keyValues);
        Task<object> FindAsync(Type entityType, object[] keyValues, CancellationToken cancellationToken);
        //TEntity Find(params object[] keyValues) where TEntity : class;
        //Task FindAsync(params object[] keyValues) where TEntity : class;
        //Task FindAsync(object[] keyValues, CancellationToken cancellationToken) where TEntity : class;
    }

    public class HealthContext : DbContext, IHealthContext
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
                optionsBuilder.UseSqlServer(@"");
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
