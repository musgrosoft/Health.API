using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Repositories.Health;
using Utils;
using Services.OAuth;
using Repositories.OAuth;
using System.Net.Http;
using Hangfire;
using Hangfire.MemoryStorage;
using HealthAPI.Hangfire;
using GoogleSheets;
using Services.Health;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Calendar = Utils.Calendar;
using Fitbit;
using HealthAPI.Importer;
using Microsoft.AspNetCore.Hosting;
using Withings;
using Microsoft.Extensions.Hosting;

namespace HealthAPI
{
   

    public class Startup
    { 
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        private readonly IConfig _config;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _config = new Config();
        }

        public IConfiguration Configuration { get; }

        public virtual void SetUpDataBase(IServiceCollection services)
        {


            services
                //.AddEntityFrameworkSqlServer()
                .AddDbContext<HealthContext>(dboptions =>
            {
                dboptions
                    .UseSqlServer(
                        _config.HealthDbConnectionString,
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);

                          //  sqlOptions.MigrationsAssembly("Repositories");

                        }
                    )
                    .EnableSensitiveDataLogging();
            });

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins(
                            "https://timsstaticwebsite.z33.web.core.windows.net",
                            "http://www.contoso.com");
                    });
            });

            services.AddControllers();
 
            SetUpDataBase(services);

            services.AddHangfire(x => x.UseMemoryStorage());

            services.AddMvc(o => { o.Filters.Add<GlobalExceptionFilter>(); });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddScoped<IHealthRepository, HealthRepository>();

            services.AddSingleton<HttpClient, HttpClient>();

            services.AddTransient<IHealthService, HealthService>();

            services.AddTransient<IConfig, Config>();

            services.AddTransient<ILogger, LogzIoLogger>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ITokenRepository, TokenRepository>();
            services.AddTransient<IFitbitAuthenticator, FitbitAuthenticator>();
            services.AddTransient<IFitbitClientQueryAdapter, FitbitClientQueryAdapter>();
            services.AddTransient<IFitbitService, FitbitService>();

            services.AddTransient<ICalendar, Calendar>();
            services.AddTransient<IWithingsClient, WithingsClient>();
            services.AddTransient<IWithingsAuthenticator, WithingsAuthenticator>();
            services.AddTransient<IFitbitClient, FitbitClient>();
            services.AddTransient<ISheetsService, SheetsService>();

            services.AddTransient<IHangfireWork, HangfireWork>();
            services.AddTransient<IWithingsService, WithingsService>();
            services.AddTransient<IFitbitMapper, FitbitMapper>();
            services.AddTransient<IFitbitWork, FitbitWork>();

            services.AddTransient<IWithingsMapper, WithingsMapper>();
            services.AddTransient<IWithingsClientQueryAdapter, WithingsClientQueryAdapter>();

            services.AddTransient<IBackgroundJobClient, BackgroundJobClient>();
            services.AddTransient<IImporter, Importer.Importer>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            //app.UseCors();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); } );
            app.UseCookiePolicy();
            app.UseHangfireDashboard();
            app.UseHangfireServer();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); } );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

        }
    }

}

