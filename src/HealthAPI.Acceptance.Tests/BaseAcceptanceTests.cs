using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Models;
using Xunit;

namespace HealthAPI.Acceptance.Tests
{
    public class BaseAcceptanceTests : IClassFixture<CustomWebApplicationFactory<HealthAPI.Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public BaseAcceptanceTests(CustomWebApplicationFactory<HealthAPI.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/swagger/index.html")]

        [InlineData("/api/Weights")]
        [InlineData("/api/BloodPressures")]

        [InlineData("/api/ActivitySummaries")]
        [InlineData("/api/HeartRateSummaries")]
        [InlineData("/api/RestingHeartRates")]
        [InlineData("/api/StepCounts")]

        [InlineData("/api/AlcoholIntakes")]
        [InlineData("/api/Ergos")]
        [InlineData("/api/Runs")]

        public async Task ShouldReturnSuccess(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldGetExpectedWeights()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/Weights");



        }

    }

    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<HealthAPI.Startup>
    {
        //protected void ConfigureServices(IServiceCollection services)
        //{

        //    services.AddDbContext<HealthContext>(options =>
        //    {
        //        options.UseInMemoryDatabase(Guid.NewGuid().ToString());
        //    });

        //    // Build the service provider.
        //    var sp = services.BuildServiceProvider();

        //    // Create a scope to obtain a reference to the database
        //    // context (ApplicationDbContext).
        //    using (var scope = sp.CreateScope())
        //    {
        //        var scopedServices = scope.ServiceProvider;
        //        var db = scopedServices.GetRequiredService<HealthContext>();
        //        var logger = scopedServices
        //            .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

        //        // Ensure the database is created.
        //        db.Database.EnsureCreated();

        //        try
        //        {
        //            // Seed the database with test data.
        //            InitializeDbForTests(db);
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.LogError(ex,
        //                $"An error occurred seeding the database with test messages. Error: {ex.Message}");
        //        }

        //    }
        //}



        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Create a new service provider.
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                // Add a database context (ApplicationDbContext) using an in-memory 
                // database for testing.
                services.AddDbContext<HealthContext>(options =>
                {
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                    options.UseInternalServiceProvider(serviceProvider);
                });

                // Build the service provider.
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<HealthContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    db.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data.
                        InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex,
                            $"An error occurred seeding the database with test messages. Error: {ex.Message}");
                    }
                }




            });
        }


        private void InitializeDbForTests(HealthContext db)
        {
            db.AddRange(new List<Weight>
            {
                new Weight {CreatedDate = new DateTime(2018,1,1), Kg = 101},
                new Weight {CreatedDate = new DateTime(2018,1,2), Kg = 102},
                new Weight {CreatedDate = new DateTime(2018,1,3), Kg = 103},
            });

            //db.Add(new Weight {CreatedDate = new DateTime(2018,1,1), Kg = 101});

            db.SaveChanges();
        }
    }
}