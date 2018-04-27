using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthAPI.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            builder.EntitySet<Weight>("Weights")
                .EntityType
                .HasKey(e => new { e.DateTime })
                .Filter(Microsoft.AspNet.OData.Query.QueryOptionSetting.Allowed) // Allow for the $filter Command
                .Count() // Allow for the $count Command
                .Expand() // Allow for the $expand Command
                .OrderBy() // Allow for the $orderby Command                
                .Page() // Allow for the $top and $skip Commands                
                .Select(); // Allow for the $select Command

            builder.EntitySet<BloodPressure>("BloodPressures")
                .EntityType
                .HasKey(e => new { e.DateTime })
                .Filter(Microsoft.AspNet.OData.Query.QueryOptionSetting.Allowed) // Allow for the $filter Command
                .Count() // Allow for the $count Command
                .Expand() // Allow for the $expand Command
                .OrderBy() // Allow for the $orderby Command                
                .Page() // Allow for the $top and $skip Commands                
                .Select(); // Allow for the $select Command

            builder.EntitySet<AlcoholIntake>("AlcoholIntakes").EntityType
                .HasKey(e => new { e.DateTime })
                .Filter(Microsoft.AspNet.OData.Query.QueryOptionSetting.Allowed) // Allow for the $filter Command
                .Count() // Allow for the $count Command
                .Expand() // Allow for the $expand Command
                .OrderBy() // Allow for the $orderby Command                
                .Page() // Allow for the $top and $skip Commands                
                .Select(); // Allow for the $select Command;
            builder.EntitySet<RestingHeartRate>("RestingHeartRates").EntityType
                .HasKey(e => new { e.DateTime })
                .Filter(Microsoft.AspNet.OData.Query.QueryOptionSetting.Allowed) // Allow for the $filter Command
                .Count() // Allow for the $count Command
                .Expand() // Allow for the $expand Command
                .OrderBy() // Allow for the $orderby Command                
                .Page() // Allow for the $top and $skip Commands                
                .Select(); // Allow for the $select Command;
            builder.EntitySet<HeartRateZoneSummary>("HeartRateDailySummaries").EntityType
                .HasKey(e => new { e.DateTime })
                .Filter(Microsoft.AspNet.OData.Query.QueryOptionSetting.Allowed) // Allow for the $filter Command
                .Count() // Allow for the $count Command
                .Expand() // Allow for the $expand Command
                .OrderBy() // Allow for the $orderby Command                
                .Page() // Allow for the $top and $skip Commands                
                .Select(); // Allow for the $select Command;

            builder.EntitySet<DailyActivity>("DailyActivities").EntityType
                .HasKey(e => new { e.DateTime })
                .Filter(Microsoft.AspNet.OData.Query.QueryOptionSetting.Allowed) // Allow for the $filter Command
                .Count() // Allow for the $count Command
                .Expand() // Allow for the $expand Command
                .OrderBy() // Allow for the $orderby Command                
                .Page() // Allow for the $top and $skip Commands                
                .Select(); // Allow for the $select Command;

            builder.EntitySet<StepCount>("Steps").EntityType
                .HasKey(e => new { e.DateTime })
                .Filter(Microsoft.AspNet.OData.Query.QueryOptionSetting.Allowed) // Allow for the $filter Command
                .Count() // Allow for the $count Command
                .Expand() // Allow for the $expand Command
                .OrderBy() // Allow for the $orderby Command                
                .Page() // Allow for the $top and $skip Commands                
                .Select(); // Allow for the $select Command;

            builder.StructuralTypes.First(t => t.ClrType == typeof(HeartRateZoneSummary)).AddProperty(typeof(HeartRateZoneSummary).GetProperty("Week"));

            builder.StructuralTypes.First(t => t.ClrType == typeof(DailyActivity)).AddProperty(typeof(DailyActivity).GetProperty("Week"));
            builder.StructuralTypes.First(t => t.ClrType == typeof(DailyActivity)).AddProperty(typeof(DailyActivity).GetProperty("ActiveMinutes"));
            
            builder.StructuralTypes.First(t => t.ClrType == typeof(StepCount)).AddProperty(typeof(StepCount).GetProperty("Week"));

            //Enabling OData routing.
            app.UseMvc(routeBuilder =>
                {
                    routeBuilder.MapODataServiceRoute("ODataRoutes", "api", builder.GetEdmModel());
                    routeBuilder.EnableDependencyInjection();
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

/*

drop table weights
drop table bloodpressures
drop table StepCounts
drop table units
drop table heartratedailysummaries
drop table RestingHeartRates
drop table DailyActivitySummaries

*/
