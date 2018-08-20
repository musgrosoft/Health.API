using Newtonsoft.Json;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HealthAPI.Acceptance.Tests
{
    public class ActivitySummariesAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
    {
        private WebApplicationFactory<Startup> _factory;

        public ActivitySummariesAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder => builder.UseStartup<TestStartup>());
        }


        [Fact]
        public async Task ShouldGetActivitySummaries()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/ActivitySummaries");

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<ActivitySummary>>(content);

            response.EnsureSuccessStatusCode();
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.VeryActiveMinutes == 11);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.VeryActiveMinutes == 12);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.VeryActiveMinutes == 13);
        }



    }
}
