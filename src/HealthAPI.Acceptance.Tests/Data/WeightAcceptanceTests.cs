//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.DependencyInjection;
//using Newtonsoft.Json;
//using Repositories;
//using Repositories.Models;
//using Xunit;

//namespace HealthAPI.Acceptance.Tests.Data
//{
//    public class WeightAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
//    {
//        private readonly WebApplicationFactory<Startup> _factory;

//        public WeightAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
//        {
//            _factory = factory.WithWebHostBuilder(builder => builder.UseStartup<TestStartup>());


//        }

//        [Fact]
//        public async Task ShouldGetWeights()
//        {
//            var client = _factory.CreateClient();
//            //client.DefaultRequestHeaders
//            //    .Accept
//            //    .Add(new MediaTypeWithQualityHeaderValue("application/json"));

//            SeedData();

//            var response = await client.GetAsync("/api/Weights");

//            var content = await response.Content.ReadAsStringAsync();
//            var data = JsonConvert.DeserializeObject<List<Weight>>(content);

//            //  Assert.Equal(960, data.Count);
//            response.EnsureSuccessStatusCode();
//            Assert.Contains(data, x => x.CreatedDate == new DateTime(2017, 1, 1) && x.Kg == 123);

//        }

//        private void SeedData()
//        {
//            using (var scope = _factory.Server.Host.Services.CreateScope())
//            {
//                var services = scope.ServiceProvider;

//                var db = (HealthContext)services.GetRequiredService(typeof(HealthContext));
//                db.Weights.Add(new Weight { CreatedDate = new DateTime(2017, 1, 1), Kg = 123 });
//                db.SaveChanges();
//            }
//        }


//    }

//}
