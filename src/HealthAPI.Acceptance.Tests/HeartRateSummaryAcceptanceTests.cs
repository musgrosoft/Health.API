using Newtonsoft.Json;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace HealthAPI.Acceptance.Tests
{
    public class HeartRateSummaryAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
    {
        private WebApplicationFactory<Startup> _factory;

        public HeartRateSummaryAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder => builder.UseStartup<TestStartup>());
        }

        [Fact]
        public async Task ShouldGetHeartRateSummaries()
        {
            var client = _factory.CreateClient();
            
            var response = await client.GetAsync("/api/HeartRateSummaries");

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<HeartRateSummary>>(content);

            Assert.Equal(3, data.Count);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.CardioMinutes == 111);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.CardioMinutes == 222);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.CardioMinutes == 333);
        }

    }
}
