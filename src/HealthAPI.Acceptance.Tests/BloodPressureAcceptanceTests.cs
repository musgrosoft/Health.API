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

    public class BloodPressureAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public BloodPressureAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder => builder.UseStartup<TestStartup>());
        }

        [Fact]
        public async Task ShouldGetBloodPressures()
        {
            var client = _factory.CreateClient();



            var response = await client.GetAsync("/api/BloodPressures");

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<BloodPressure>>(content);

            //  Assert.Equal(960, data.Count);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Diastolic == 81);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Diastolic == 82);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Diastolic == 83);
        }


        //'http://musgrosoft-health-api.azurewebsites.net/odata/BloodPressures';
        //"http://musgrosoft-health-api.azurewebsites.net/odata/BloodPressures?$top=1&$orderby=DateTime%20desc";

    }
}