//using System.Diagnostics;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Xunit;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.DependencyInjection;
//using Services.Fitbit;

//namespace HealthAPI.Acceptance.Tests
//{
//    public class WithingsAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
//    {
//        private readonly WebApplicationFactory<Startup> _factory;

//        public WithingsAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
//        {
//            _factory = factory.WithWebHostBuilder(builder => builder.UseStartup<TestStartup>());
//        }

//        [Fact]
//        public async Task ShouldListSubscriptions()
//        {
//            //var client = _factory.CreateClient();

//            //var response = await client.GetAsync("/api/Withings/ListSubscriptions");

//            //Assert.Equal("200",response.StatusCode.ToString());


//        }



//    }

//}
