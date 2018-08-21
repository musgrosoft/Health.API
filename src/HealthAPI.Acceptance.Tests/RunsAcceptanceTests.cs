using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Repositories;
using Repositories.Models;
using Xunit;

namespace HealthAPI.Acceptance.Tests
{
    public class RunsAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
    {
        private WebApplicationFactory<Startup> _factory;

        public RunsAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder => builder.UseStartup<TestStartup>());
        }

        [Fact]
        public async Task ShouldGetErgos()
        {
            var client = _factory.CreateClient();

            SeedData();

            var response = await client.GetAsync("/api/Runs");

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Run>>(content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(3, data.Count);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Metres == 101);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Metres == 102);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Metres == 103);
        }

        private void SeedData()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var db = (HealthContext)services.GetRequiredService(typeof(HealthContext));



                db.Runs.AddRange(new List<Run> {
                    new Run{ CreatedDate = new DateTime(2018,1,1), Metres = 101},
                    new Run{ CreatedDate = new DateTime(2018,1,2), Metres = 102},
                    new Run{ CreatedDate = new DateTime(2018,1,3), Metres = 103},

                });

                db.SaveChanges();
            }
        }
    }
}
