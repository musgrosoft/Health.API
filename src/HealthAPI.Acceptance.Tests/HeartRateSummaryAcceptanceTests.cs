//using HealthAPI.Acceptance.Tests.OData;
//using Newtonsoft.Json;
//using Repositories;
//using Repositories.Health;
//using Repositories.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using Utils;
//using Xunit;

//namespace HealthAPI.Acceptance.Tests
//{
//    public class HeartRateSummaryAcceptanceTests
//    {
//        private HttpClient _httpClient;

//        public HeartRateSummaryAcceptanceTests()
//        {
//            _httpClient = new HttpClient();
//        }

//        [Fact]
//        public async Task ShouldGetHeartRateSummaries()
//        {
//            var config = new Config();
//            var healthContext = new HealthContext(config);
//            var healthRepository = new HealthRepository(healthContext);

//            var allHeartRateSummaries = healthRepository.GetAllHeartRateSummaries();
//            var countHeartRateSummaries = allHeartRateSummaries.Count();

//            var heartRateSummaries = await GetHeartRateSummariesFromAPI();

//            Assert.Equal(countHeartRateSummaries, heartRateSummaries.Count());

//            //var lastStepCount = data.value.OrderByDescending(x => x.DateTime).FirstOrDefault();
//            //Assert.True(lastStepCount.Count < 30000);
//            //Assert.True(lastStepCount.Count > 10);

//            var heartRateSummary = new HeartRateSummary
//            {
//                DateTime = new DateTime(1970, 1, 1),
//                CardioMinutes = 123
//            };

//            healthRepository.Upsert(heartRateSummary);

//            heartRateSummaries = await GetHeartRateSummariesFromAPI();
//            Assert.Equal(countHeartRateSummaries + 1, heartRateSummaries.Count());

//            healthRepository.Delete(heartRateSummary);

//            heartRateSummaries = await GetHeartRateSummariesFromAPI();
//            Assert.Equal(countHeartRateSummaries, heartRateSummaries.Count());
//        }

//        private async Task<IEnumerable<HeartRateSummary>> GetHeartRateSummariesFromAPI()
//        {
//            var uri = "http://musgrosoft-health-api.azurewebsites.net/odata/HeartRateSummaries";
//            _httpClient.DefaultRequestHeaders.Clear();

//            var response = await _httpClient.GetAsync(uri);
//            Assert.True(response.IsSuccessStatusCode);

//            var content = await response.Content.ReadAsStringAsync();
//            var data = JsonConvert.DeserializeObject<ODataResponse<HeartRateSummary>>(content);
//            Assert.NotNull(data);

//            return data.value;
//        }


////'https://musgrosoft-health-api.azurewebsites.net/odata/HeartRateSummaries/GroupByMonth';
////'https://musgrosoft-health-api.azurewebsites.net/odata/HeartRateSummaries/GroupByWeek';
//    }
//}
