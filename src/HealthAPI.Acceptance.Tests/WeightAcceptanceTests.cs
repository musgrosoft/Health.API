using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Repositories;
using Xunit;
using Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAPI.Acceptance.Tests
{
    public class WeightAcceptanceTests : IClassFixture<WebApplicationFactory<HealthAPI.Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public WeightAcceptanceTests(WebApplicationFactory<HealthAPI.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ShouldGetWeights()
        {
            var client = _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddScoped<HealthContext, FakeLocalContext>();
                    });
                })
                .CreateClient();


            var response = await client.GetAsync("/api/Weights");

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Weight>>(content);

            Assert.Equal(3, data.Count);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 1) && x.Kg == 101);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 2) && x.Kg == 102);
            Assert.Contains(data, x => x.CreatedDate == new DateTime(2018, 1, 3) && x.Kg == 103);

        }

        private void InitializeDbForTests(HealthContext db)
        {
            db.AddRange(new List<Weight>
            {
                new Weight {CreatedDate = new DateTime(2018,1,1), Kg = 101},
                new Weight {CreatedDate = new DateTime(2018,1,2), Kg = 102},
                new Weight {CreatedDate = new DateTime(2018,1,3), Kg = 103},
            });

            //db.Add(new Weight {CreatedDate = new DateTime(2018,1,1), Kg = 101});

            db.SaveChanges();
        }

    }

    public class FakeLocalContext : HealthContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            }
        }


        //public FakeLocalContext() : base(new Config())
        //{
        //}

        public FakeLocalContext() : base(new DbContextOptions<HealthContext>())
        {
        }
    }
}
