using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Repositories;
using Services.Fitbit;

namespace HealthAPI.Acceptance.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void SetUpDataBase(IServiceCollection services)
        {
            services.AddDbContext<HealthContext>(options =>
            {
                options.UseInMemoryDatabase("fake");
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            
            var descriptorFitbitService = new ServiceDescriptor(
                typeof(IFitbitService),
                typeof(FitbitServiceStub),
                ServiceLifetime.Singleton);

            services.Replace(descriptorFitbitService);

        }
    }

}

