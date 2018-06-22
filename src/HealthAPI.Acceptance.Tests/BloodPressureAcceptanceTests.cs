using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HealthAPI.Acceptance.Tests.Domain;
using Newtonsoft.Json;
using Xunit;

namespace HealthAPI.Acceptance.Tests
{

    public class BloodPressureAcceptanceTests
    {
        private HttpClient _httpClient;

        public BloodPressureAcceptanceTests()
        {
            _httpClient = new HttpClient();
        }

        [Fact]
        public async Task ShouldGetBloodPressures()
        {
            var uri = "http://musgrosoft-health-api.azurewebsites.net/odata/BloodPressures";
            _httpClient.DefaultRequestHeaders.Clear();

            var response = await _httpClient.GetAsync(uri);
            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ODataResponse<BloodPressure>>(content);
            Assert.NotNull(data);
            Assert.True(data.value.Count > 100);

            var lastBloodPressure = data.value.OrderByDescending(x => x.DateTime).FirstOrDefault();
            Assert.True(lastBloodPressure.Systolic < 160);
            Assert.True(lastBloodPressure.Systolic > 100);
        }


//'http://musgrosoft-health-api.azurewebsites.net/odata/BloodPressures';
//"http://musgrosoft-health-api.azurewebsites.net/odata/BloodPressures?$top=1&$orderby=DateTime%20desc";

    }
}