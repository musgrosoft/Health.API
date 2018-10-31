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
//    public class AlcoholIntakesAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
//    {
//        private WebApplicationFactory<Startup> _factory;

//        public AlcoholIntakesAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
//        {
//            _factory = factory.WithWebHostBuilder(builder => builder.UseStartup<TestStartup>());
//        }

//        [Fact]
//        public async Task ShouldGetAlcoholIntakes()
//        {
//            var client = _factory.CreateClient();
//            client.DefaultRequestHeaders
//                .Accept
//                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

//            SeedData();
            
//            var response = await client.GetAsync("/api/AlcoholIntakes");

//            var content = await response.Content.ReadAsStringAsync();
//            var data = JsonConvert.DeserializeObject<List<AlcoholIntake>>(content);

//            response.EnsureSuccessStatusCode();
//            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Units == 1);
//            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Units == 2);
//            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Units == 3);
//        }

//        //'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes?$filter=year(DateTime)%20eq%202016';
//        //'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes?$filter=year(DateTime)%20eq%202017';
//        //'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes?$filter=year(DateTime)%20eq%202018';
//        //'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes/GroupByMonth';
//        //'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes/GroupByWeek';
//        private void SeedData()
//        {
//            using (var scope = _factory.Server.Host.Services.CreateScope())
//            {
//                var services = scope.ServiceProvider;

//                var db = (HealthContext)services.GetRequiredService(typeof(HealthContext));

                

//                db.AlcoholIntakes.AddRange(new List<AlcoholIntake> {
//                    new AlcoholIntake{ CreatedDate = new DateTime(2018,1,1), Units = 1},
//                    new AlcoholIntake{ CreatedDate = new DateTime(2018,1,2), Units = 2},
//                    new AlcoholIntake{ CreatedDate = new DateTime(2018,1,3), Units = 3},

//                });

//                db.SaveChanges();
//            }
//        }

//    }
//}
