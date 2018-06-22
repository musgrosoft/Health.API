using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HealthAPI.Acceptance.Tests.Domain;
using Newtonsoft.Json;
using Xunit;

namespace HealthAPI.Acceptance.Tests
{
    public class WeightAcceptanceTests
    {
        private HttpClient _httpClient;

        public WeightAcceptanceTests()
        {
            _httpClient = new HttpClient();
        }

        [Fact]
        public async Task ShouldGetWeights()
        {
            var uri = "http://musgrosoft-health-api.azurewebsites.net/odata/Weights";
            _httpClient.DefaultRequestHeaders.Clear();

            var response = await _httpClient.GetAsync(uri);
            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ODataResponse<Weight>>(content);
            Assert.NotNull(data);
            Assert.True(data.value.Count > 860);

            var lastWeight = data.value.OrderByDescending(x=>x.DateTime).FirstOrDefault();
            Assert.True(lastWeight.Kg < 100);
            Assert.True(lastWeight.Kg > 80);
        }

        

        //'http://musgrosoft-health-api.azurewebsites.net/odata/Weights';
        //"http://musgrosoft-health-api.azurewebsites.net/odata/Weights?$top=1&$orderby=DateTime%20desc";
        //"http://musgrosoft-health-api.azurewebsites.net/odata/Weights?$top=1&$orderby=DateTime%20desc&$filter=MovingAverageKg%20lt%2088.7";
        //"http://musgrosoft-health-api.azurewebsites.net/odata/Weights?$top=1&$orderby=DateTime%20desc&$filter=Kg%20lt%2088.7";


    }
}
