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
//    public class ActivitySummariesAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
//    {
//        private WebApplicationFactory<Startup> _factory;

//        public ActivitySummariesAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
//        {
//            _factory = factory.WithWebHostBuilder(builder => builder.UseStartup<TestStartup>());
//        }


//        [Fact]
//        public async Task ShouldGetActivitySummaries()
//        {
//            var client = _factory.CreateClient();
//            client.DefaultRequestHeaders
//                .Accept
//                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

//            SeedData();

//            var response = await client.GetAsync("/api/ActivitySummaries");

//            var content = await response.Content.ReadAsStringAsync();
//            var data = JsonConvert.DeserializeObject<List<ActivitySummary>>(content);

//            response.EnsureSuccessStatusCode();
//            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.VeryActiveMinutes == 11);
//            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.VeryActiveMinutes == 12);
//            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.VeryActiveMinutes == 13);
//        }

//        private void SeedData()
//        {
//            using (var scope = _factory.Server.Host.Services.CreateScope())
//            {
//                var services = scope.ServiceProvider;

//                var db = (HealthContext)services.GetRequiredService(typeof(HealthContext));



//                db.ActivitySummaries.AddRange(new List<ActivitySummary> {
//                    new ActivitySummary{ CreatedDate = new DateTime(2018,1,1), VeryActiveMinutes = 11},
//                    new ActivitySummary{ CreatedDate = new DateTime(2018,1,2), VeryActiveMinutes = 12},
//                    new ActivitySummary{ CreatedDate = new DateTime(2018,1,3), VeryActiveMinutes = 13},

//                });

//                db.SaveChanges();
//            }
//        }


//    }
//}
