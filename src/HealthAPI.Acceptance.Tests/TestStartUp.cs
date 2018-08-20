using System;
using System.Net.Http;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Migrators;
using Migrators.Google;
using Migrators.Hangfire;
using Migrators.Nokia;
using Repositories;
using Repositories.Health;
using Repositories.Models;
using Repositories.OAuth;
using Services;
using Services.Fitbit;
using Services.Fitbit.Domain;
using Services.Google;
using Services.Health;
using Services.Nokia;
using Services.OAuth;
using Swashbuckle.AspNetCore.Swagger;
using Utils;

namespace HealthAPI.Acceptance.Tests
{
    public class TestStartup
    {

        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new Config();

            //services.AddDbContext<HealthContext>(options =>
            //{
            //options.UseInMemoryDatabase("fake");// Guid.NewGuid().ToString());
            //});

            services.AddDbContext<HealthContext>(options =>
            {
                options.UseInMemoryDatabase("fake");
            });

            services.AddHangfire(x => x.UseMemoryStorage());

            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddMvc(o => { o.Filters.Add<GlobalExceptionFilter>(); });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddScoped<IHealthRepository, HealthRepository>();

            services.AddSingleton<HttpClient, HttpClient>();

            services.AddTransient<IHealthService, HealthService>();
            services.AddTransient<IConfig, Config>();
            services.AddTransient<ILogger, Logger>();
            services.AddTransient<IAggregateStatisticsCalculator, AggregateStatisticsCalculator>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<IFitbitAuthenticator, FitbitAuthenticator>();
            services.AddTransient<IFitbitClientQueryAdapter, FitbitClientQueryAdapter>();
            services.AddTransient<IFitbitService, FitbitService>();
            services.AddTransient<IFitbitMigrator, FitbitMigrator>();
            services.AddTransient<ICalendar, Calendar>();
            services.AddTransient<INokiaMigrator, NokiaMigrator>();
            services.AddTransient<INokiaClient, NokiaClient>();
            services.AddTransient<INokiaAuthenticator, NokiaAuthenticator>();
            services.AddTransient<IFitbitClient, FitbitClient>();
            services.AddTransient<ITargetService, TargetService>();
            services.AddTransient<IEntityAggregator, EntityAggregator>();
            services.AddTransient<IEntityDecorator, EntityDecorator>();
            services.AddTransient<IGoogleClient, GoogleClient>();
            services.AddTransient<IGoogleMigrator, GoogleMigrator>();
            services.AddTransient<IHangfireUtility, HangfireUtility>();
            services.AddTransient<IHangfireWork, HangfireWork>();
            services.AddTransient<INokiaService, NokiaService>();
            services.AddTransient<ITargetCalculator, TargetCalculator>();
            services.AddTransient<IFitbitMapper, FitbitMapper>();



            //// ********************
            //// Setup CORS
            //// ********************
            //var corsBuilder = new CorsPolicyBuilder();
            //corsBuilder.AllowAnyHeader();
            //corsBuilder.AllowAnyMethod();
            //corsBuilder.AllowAnyOrigin(); // For anyone access.
            ////corsBuilder.WithOrigins("http://localhost:56573"); // for a specific url. Don't add a forward slash on the end!
            ////corsBuilder.AllowCredentials();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("SiteCorsPolicy", corsBuilder.Build());
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //Hangfire.GlobalConfiguration.Configuration.UseMemoryStorage();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            //app.UseCors(builder => builder.WithOrigins("http://www.musgrosoft.co.uk"));

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<HealthContext>();
                //context.Database.Migrate();
                
                //context.Database.EnsureCreated();
                context.EnsureSeedData();



            }

            //var db = new HealthContext(new DbContextOptionsBuilder<HealthContext>()
            //    .UseInMemoryDatabase(databaseName: "fake")
            //    .Options);

            //db.Weights.Add(new Weight { CreatedDate = new DateTime(2010, 1, 1), Kg = 100 });
            //db.SaveChanges();

        }
    }

    public class FakeLocalContext : HealthContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            }

        }


        //public FakeLocalContext() : base(new Config())
        //{
        //}

        public FakeLocalContext() : base(new DbContextOptions<HealthContext>())
        {

            this.Weights.Add(new Weight { CreatedDate = new DateTime(2018, 1, 1), Kg = 123 });
            this.SaveChanges();
        }
    }
}

