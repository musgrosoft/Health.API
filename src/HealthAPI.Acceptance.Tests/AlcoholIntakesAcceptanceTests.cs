using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Repositories.Models;

namespace HealthAPI.Acceptance.Tests
{
    public class AlcoholIntakesAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
    {
        private WebApplicationFactory<Startup> _factory;

        public AlcoholIntakesAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder => builder.UseStartup<TestStartup>());
        }

        [Fact]
        public async Task ShouldGetAlcoholIntakes()
        {
            var client = _factory.CreateClient();
            
            var response = await client.GetAsync("/api/AlcoholIntakes");

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<AlcoholIntake>>(content);

            response.EnsureSuccessStatusCode();
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Units == 1);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Units == 2);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Units == 3);
        }

        //'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes?$filter=year(DateTime)%20eq%202016';
        //'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes?$filter=year(DateTime)%20eq%202017';
        //'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes?$filter=year(DateTime)%20eq%202018';
        //'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes/GroupByMonth';
        //'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes/GroupByWeek';


    }
}
