using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Repositories.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Collections.Generic;
using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Repositories;

namespace HealthAPI.Acceptance.Tests
{
    public class StepCountAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public StepCountAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder => builder.UseStartup<TestStartup>());
        }

        [Fact]
        public async Task ShouldGetStepCounts()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));


            SeedData();

            var response = await client.GetAsync("/api/StepCounts");
            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<StepCount>>(content);

            response.EnsureSuccessStatusCode();
            Assert.NotNull(data);
            Assert.True(data.Count == 3);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Count == 1111);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Count == 2222);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Count == 3333);
        }


        private void SeedData()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var db = (HealthContext)services.GetRequiredService(typeof(HealthContext));



                db.StepCounts.AddRange(new List<StepCount> {
                    new StepCount{ CreatedDate = new DateTime(2018,1,1), Count = 1111},
                    new StepCount{ CreatedDate = new DateTime(2018,1,2), Count = 2222},
                    new StepCount{ CreatedDate = new DateTime(2018,1,3), Count = 3333},

                });

                db.SaveChanges();
            }
        }
    }
}