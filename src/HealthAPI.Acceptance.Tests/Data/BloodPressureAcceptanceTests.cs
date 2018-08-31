using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Repositories;
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
            client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            SeedData();

            var response = await client.GetAsync("/api/BloodPressures");

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<BloodPressure>>(content);

            response.EnsureSuccessStatusCode();
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Diastolic == 81);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Diastolic == 82);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Diastolic == 83);
        }


        //'http://musgrosoft-health-api.azurewebsites.net/odata/BloodPressures';
        //"http://musgrosoft-health-api.azurewebsites.net/odata/BloodPressures?$top=1&$orderby=DateTime%20desc";

    

        private void SeedData()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
            var services = scope.ServiceProvider;

            var db = (HealthContext)services.GetRequiredService(typeof(HealthContext));



            db.BloodPressures.AddRange(new List<BloodPressure> {
                    new BloodPressure{ CreatedDate = new DateTime(2018,1,1), Diastolic = 81},
                    new BloodPressure{ CreatedDate = new DateTime(2018,1,2), Diastolic = 82},
                    new BloodPressure{ CreatedDate = new DateTime(2018,1,3), Diastolic = 83},

                });

                db.SaveChanges();
            }
        }

    }
}