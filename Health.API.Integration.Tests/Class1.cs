using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Health.API.Integration.Tests
{
    public class Class1 : IClassFixture<WebApplicationFactory<Health.API.Controllers.CanaryController>>
    {
        private readonly WebApplicationFactory<Health.API.Controllers.CanaryController> _factory;

        public Class1(WebApplicationFactory<Health.API.Controllers.CanaryController> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task thing()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/");

            //Then
            response.EnsureSuccessStatusCode();
            Assert.Equal("Hello Tim", await response.Content.ReadAsStringAsync());
        }
    }
}
