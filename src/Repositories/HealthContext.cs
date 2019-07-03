using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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


                modelBuilder.Entity<RestingHeartRate>().HasData(new RestingHeartRate { CreatedDate = new DateTime(2015, 1, 1), Beats = 123 });
                modelBuilder.Entity<RestingHeartRate>().HasData(new RestingHeartRate { CreatedDate = new DateTime(2015, 1, 2), Beats = 123 });

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

                try
                {
                    this.Database.ExecuteSqlCommand(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Sql Scripts/vw_Weights_Daily.sql"));
                }
                catch(Exception ex)
                {
                    Task task = Task.Run(async () => await _logger.LogErrorAsync(ex));
                    //return task.Result;

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
            return (int)(3386 + (DateTime.Now.Date - new DateTime(2019, 1, 1)).TotalDays);
        }

        private int GetTargetMetresFor30MinuteTreadmill(DateTime date)
        {
            return (int)(5000 + 2.74725275 * (DateTime.Now.Date - new DateTime(2019, 1, 1)).TotalDays);
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
