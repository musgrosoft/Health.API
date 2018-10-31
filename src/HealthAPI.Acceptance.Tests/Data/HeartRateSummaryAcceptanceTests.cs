//using System;
//using System.Collections.Generic;
//using System.Net.Http.Headers;
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
//    public class HeartRateSummaryAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
//    {
//        private WebApplicationFactory<Startup> _factory;

//        public HeartRateSummaryAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
//        {
//            _factory = factory.WithWebHostBuilder(builder => builder.UseStartup<TestStartup>());
//        }

//        [Fact]
//        public async Task ShouldGetHeartRateSummaries()
//        {
//            var client = _factory.CreateClient();
//            client.DefaultRequestHeaders
//                .Accept
//                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

//            SeedData();
            
//            var response = await client.GetAsync("/api/HeartRateSummaries");

//            var content = await response.Content.ReadAsStringAsync();
//            var data = JsonConvert.DeserializeObject<List<HeartRateSummary>>(content);

//            response.EnsureSuccessStatusCode();
//            Assert.Equal(3, data.Count);
//            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.CardioMinutes == 111);
//            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.CardioMinutes == 222);
//            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.CardioMinutes == 333);
//        }


//        private void SeedData()
//        {
//            using (var scope = _factory.Server.Host.Services.CreateScope())
//            {
//                var services = scope.ServiceProvider;

//                var db = (HealthContext)services.GetRequiredService(typeof(HealthContext));



//                db.HeartRateSummaries.AddRange(new List<HeartRateSummary> {
//                    new HeartRateSummary{ CreatedDate = new DateTime(2018,1,1), CardioMinutes = 111},
//                    new HeartRateSummary{ CreatedDate = new DateTime(2018,1,2), CardioMinutes = 222},
//                    new HeartRateSummary{ CreatedDate = new DateTime(2018,1,3), CardioMinutes = 333},

//                });

//                db.SaveChanges();
//            }
//        }
//    }
//}
