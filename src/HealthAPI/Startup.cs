using System.Linq;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Repositories.Models;
using Repositories.Health;
using Services.MyHealth;
using Utils;
using Services.OAuth;
using Repositories.OAuth;
using Services.Fitbit;
using System.Net.Http;
using Services;
using Migrators;
using Services.Nokia;

namespace HealthAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HealthContext>();

            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddOData();
            services.AddMvc();

            services.AddScoped<IHealthRepository, HealthRepository>();

            services.AddSingleton<HttpClient, HttpClient>();

            services.AddTransient<IHealthService, HealthService>();
            services.AddTransient<IConfig, Config>();
            services.AddTransient<ILogger, Logger>();
            services.AddTransient<IAggregationCalculator, AggregationCalculator>();
            services.AddTransient<IOAuthService, OAuthService>();

            services.AddTransient<IOAuthTokenRepository, OAuthTokenRepository>();

            services.AddTransient<IFitbitAuthenticator, FitbitAuthenticator>();
            services.AddTransient<IFitbitClientAggregator, FitbitClientAggregator>();
            services.AddTransient<IFitbitService, FitbitService>();
            services.AddTransient<IFitbitMigrator, FitbitMigrator>();
            services.AddTransient<ICalendar, Calendar>();

            services.AddTransient<IOAuthTokenRepository, OAuthTokenRepository>();

            services.AddTransient<INokiaMigrator, NokiaMigrator>();
            services.AddTransient<INokiaClient, NokiaClient>();
            services.AddTransient<IFitbitClient, FitbitClient>();

            services.AddTransient<ITargetService, TargetService>();

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
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseMvc();


            //Adding Model class to OData
            var builder = new ODataConventionModelBuilder();


            builder.EntitySet<AlcoholIntake>("AlcoholIntakes").EntityType
                .HasKey(e => new { e.CreatedDate })
                .Filter(Microsoft.AspNet.OData.Query.QueryOptionSetting.Allowed) // Allow for the $filter Command
                .Count() // Allow for the $count Command
                .Expand() // Allow for the $expand Command
                .OrderBy() // Allow for the $orderby Command                
                .Page() // Allow for the $top and $skip Commands                
                .Select(); // Allow for the $select Command;


            builder.EntitySet<HeartRateSummary>("HeartRateSummaries").EntityType
                .HasKey(e => new { e.CreatedDate })
                .Filter(Microsoft.AspNet.OData.Query.QueryOptionSetting.Allowed) // Allow for the $filter Command
                .Count() // Allow for the $count Command
                .Expand() // Allow for the $expand Command
                .OrderBy() // Allow for the $orderby Command                
                .Page() // Allow for the $top and $skip Commands                
                .Select(); // Allow for the $select Command;



            builder.StructuralTypes.First(t => t.ClrType == typeof(ActivitySummary)).AddProperty(typeof(ActivitySummary).GetProperty("ActiveMinutes"));


            //app.UseMvc(route =>
            //{
            //    route.MapRoute(
            //      name: "api",
            //      template: "api/{controller=Home}/{action=Index}/{id?}"
            //    );

            //    //route.MapRoute(
            //    //  name: "areas",
            //    //  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
            //    //);
            //});

            //Enabling OData routing.
            app.UseMvc(routeBuilder =>
                {
                    //routebuilder.Filter().Expand().Select().OrderBy().MaxTop(null).Count();
                    routeBuilder.MapODataServiceRoute("ODataRoutes", "odata", builder.GetEdmModel());
                    routeBuilder.EnableDependencyInjection();
                    routeBuilder.MapRoute(
                  name: "api",
                  template: "api/{controller=Home}/{action=Index}/{id?}"
                );
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

