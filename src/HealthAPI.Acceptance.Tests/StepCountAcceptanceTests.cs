//using System.Linq;
//using System.Net.Http;
//using System.Threading.Tasks;
//using HealthAPI.Acceptance.Tests.OData;
//using Newtonsoft.Json;
//using Xunit;
//using Repositories.Models;

//namespace HealthAPI.Acceptance.Tests
//{
//    public class StepCountAcceptanceTests
//    {
//        private HttpClient _httpClient;

//        public StepCountAcceptanceTests()
//        {
//            _httpClient = new HttpClient();
//        }

//        [Fact]
//        public async Task ShouldGetStepCounts()
//        {
//            var uri = "http://musgrosoft-health-api.azurewebsites.net/odata/StepCounts";
//            _httpClient.DefaultRequestHeaders.Clear();

//            var response = await _httpClient.GetAsync(uri);
//            Assert.True(response.IsSuccessStatusCode);

//            var content = await response.Content.ReadAsStringAsync();
//            var data = JsonConvert.DeserializeObject<ODataResponse<StepCount>>(content);
//            Assert.NotNull(data);
//            Assert.True(data.value.Count > 100);

//            var lastStepCount = data.value.OrderByDescending(x => x.DateTime).FirstOrDefault();
//            Assert.True(lastStepCount.Count < 30000);
//            Assert.True(lastStepCount.Count > 10);
//        }


////'http://musgrosoft-health-api.azurewebsites.net/odata/StepCounts/GroupByMonth';
////'http://musgrosoft-health-api.azurewebsites.net/odata/StepCounts/GroupByWeek';
//    }
//}