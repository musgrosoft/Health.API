using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Util;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Migrators;
using Migrators.Google;
using Migrators.Hangfire;
using Migrators.Nokia;
using Repositories;
using Repositories.Health;
using Repositories.OAuth;
using Services;
using Services.Fitbit;
using Services.Google;
using Services.Health;
using Services.Nokia;
using Services.OAuth;
using Swashbuckle.AspNetCore.Swagger;
using Utils;
using Xunit;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace HealthAPI.Acceptance.Tests
{
    public class ExperimentalTests : IClassFixture<CustomWebApplicationFactory<HealthAPI.Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public ExperimentalTests(CustomWebApplicationFactory<HealthAPI.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/swagger/index.html")]
        [InlineData("/api/weights")]
        public async Task ShouldReturnSuccess(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }

    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<HealthAPI.Startup>
    {
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
                      //  Utilities.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, $"An error occurred seeding the database with test messages. Error: {ex.Message}");
                    }
                }



                //services.AddDbContext<HealthContext>();

                //services.AddHangfire(x => x.UseMemoryStorage());

                //// Add service and create Policy with options
                //services.AddCors(options =>
                //{
                //    options.AddPolicy("CorsPolicy",
                //        b => b.AllowAnyOrigin()
                //            .AllowAnyMethod()
                //            .AllowAnyHeader()
                //            .AllowCredentials());
                //});

                //services.AddMvc(o => { o.Filters.Add<GlobalExceptionFilter>(); });

                //services.AddSwaggerGen(c =>
                //{
                //    c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                //});

                //services.AddScoped<IHealthRepository, HealthRepository>();

                //services.AddSingleton<HttpClient, HttpClient>();

                //services.AddTransient<IHealthService, HealthService>();
                //services.AddTransient<IConfig, Config>();
                //services.AddTransient<Utils.ILogger, Logger>();
                //services.AddTransient<IAggregateStatisticsCalculator, AggregateStatisticsCalculator>();
                //services.AddTransient<ITokenService, TokenService>();
                //services.AddTransient<ITokenRepository, TokenRepository>();
                //services.AddTransient<IFitbitAuthenticator, FitbitAuthenticator>();
                //services.AddTransient<IFitbitClientQueryAdapter, FitbitClientQueryAdapter>();
                //services.AddTransient<IFitbitService, FitbitService>();
                //services.AddTransient<IFitbitMigrator, FitbitMigrator>();
                //services.AddTransient<ICalendar, Calendar>();
                //services.AddTransient<INokiaMigrator, NokiaMigrator>();
                //services.AddTransient<INokiaClient, NokiaClient>();
                //services.AddTransient<INokiaAuthenticator, NokiaAuthenticator>();
                //services.AddTransient<IFitbitClient, FitbitClient>();
                //services.AddTransient<ITargetService, TargetService>();
                //services.AddTransient<IEntityAggregator, EntityAggregator>();
                //services.AddTransient<IEntityDecorator, EntityDecorator>();
                //services.AddTransient<IGoogleClient, GoogleClient>();
                //services.AddTransient<IGoogleMigrator, GoogleMigrator>();
                //services.AddTransient<IHangfireUtility, HangfireUtility>();
                //services.AddTransient<IHangfireWork, HangfireWork>();
                //services.AddTransient<INokiaService, NokiaService>();
                //services.AddTransient<ITargetCalculator, TargetCalculator>();



            });
        }

    }
}