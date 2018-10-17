using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Repositories.Health;
using Utils;
using Services.OAuth;
using Repositories.OAuth;
using System.Net.Http;
using Fitbit.Migrator;
using Fitbit.Services;
using Google;
//using Services.Google;
using Nokia;
using Hangfire;
using HealthAPI.Hangfire;
using Services.Health;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Nokia.Migrator;
using Nokia.Services;

namespace HealthAPI
{
    public class Startup
    {
        private Config _config;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _config = new Config();
        }

        public IConfiguration Configuration { get; }

        public virtual void SetUpDataBase(IServiceCollection services)
        {
            

            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<HealthContext>(dboptions =>
            {
                dboptions.UseSqlServer(
                    _config.HealthDbConnectionString,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);

                        sqlOptions.MigrationsAssembly("Repositories");
                    }
                );
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            SetUpDataBase(services);

            services.AddHangfire(x => x.UseSqlServerStorage(_config.HealthDbConnectionString));

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

            services.AddTransient<IHealthConfig, Config>();
            services.AddTransient<IFitbitConfig, Config>();
            services.AddTransient<INokiaConfig, Config>();
            services.AddTransient<IGoogleSheetsConfig, Config>();


            services.AddTransient<ILogger, LogzIoLogger>();
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
           // services.AddTransient<IHangfireUtility, HangfireUtility>();
            services.AddTransient<IHangfireWork, HangfireWork>();
            services.AddTransient<INokiaService, NokiaService>();
            services.AddTransient<ITargetCalculator, TargetCalculator>();
            services.AddTransient<IFitbitMapper, FitbitMapper>();

            services.AddTransient<INokiaMapper, NokiaMapper>();
            services.AddTransient<INokiaClientQueryAdapter, NokiaClientQueryAdapter>();

            services.AddTransient<IBackgroundJobClient, BackgroundJobClient>();


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
                // context.EnsureSeedData();
                context.Database.EnsureCreated();
                
            }

        }
    }
}

