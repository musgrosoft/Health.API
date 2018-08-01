using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Repositories.Health;
using Services.MyHealth;
using Utils;
using Services.OAuth;
using Repositories.OAuth;
using Services.Fitbit;
using System.Net.Http;
using Services;
using Migrators;
using Services.Google;
using Services.Nokia;
using Hangfire.MemoryStorage;
using Hangfire;
using Swashbuckle.AspNetCore.Swagger;

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
            services.AddHangfire(x => x.UseMemoryStorage());

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

            //services.AddMvc();
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
            services.AddTransient<IOAuthService, OAuthService>();

            services.AddTransient<IOAuthTokenRepository, TokenRepository>();

            services.AddTransient<IFitbitAuthenticator, FitbitAuthenticator>();
            services.AddTransient<IFitbitClientAggregator, FitbitClientAggregator>();
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

            //app.UseMvc(routeBuilder =>
            //    {

            //     //   routeBuilder.EnableDependencyInjection();
            //        routeBuilder.MapRoute(
            //      name: "api",
            //      template: "api/{controller=Home}/{action=Index}/{id?}"
            //    );
            //    });

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

