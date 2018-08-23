using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Services.Fitbit;

namespace HealthAPI.Acceptance.Tests
{
    public class FitbitAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public FitbitAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder => builder.UseStartup<TestStartup>());
        }

        [Fact]
        public async Task ShouldSubscribe()
        {
            var client = _factory.CreateClient();
            
            var response = await client.GetAsync("/api/Fitbit/Subscribe");

            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var fitbitService = (FitbitServiceStub)services.GetRequiredService(typeof(IFitbitService));

                Assert.True(fitbitService.HasSubscribed);
            }


        }



    }

}
