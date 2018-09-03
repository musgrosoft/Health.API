using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Xunit;
using Repositories.Models;

namespace HealthAPI.Acceptance.Tests
{
    public class RestingHeartRateAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
    {
        private WebApplicationFactory<Startup> _factory;

        public RestingHeartRateAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder => builder.UseStartup<TestStartup>());
        }

        [Fact]
        public async Task ShouldGetRestingHeartRates()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            SeedData();

            var response = await client.GetAsync("/api/RestingHeartRates");

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<RestingHeartRate>>(content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(3, data.Count);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Beats == 101);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Beats == 102);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Beats == 103);
        }

        private void SeedData()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var db = (HealthContext)services.GetRequiredService(typeof(HealthContext));



                db.RestingHeartRates.AddRange(new List<RestingHeartRate> {
                    new RestingHeartRate{ CreatedDate = new DateTime(2018,1,1), Beats = 101},
                    new RestingHeartRate{ CreatedDate = new DateTime(2018,1,2), Beats = 102},
                    new RestingHeartRate{ CreatedDate = new DateTime(2018,1,3), Beats = 103},

                });

                db.SaveChanges();
            }
        }
    }
}
