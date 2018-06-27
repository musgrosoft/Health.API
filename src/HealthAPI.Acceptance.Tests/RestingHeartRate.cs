//using HealthAPI.Acceptance.Tests.OData;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;
//using Repositories.Models;

//namespace HealthAPI.Acceptance.Tests
//{
//    public class RestingHeartRateAcceptanceTests
//    {
//        private HttpClient _httpClient;

//        public RestingHeartRateAcceptanceTests()
//        {
//            _httpClient = new HttpClient();
//        }

//        [Fact]
//        public async Task ShouldGetRestingHeartRates()
//        {
//            var uri = "http://musgrosoft-health-api.azurewebsites.net/odata/RestingHeartRates";
//            _httpClient.DefaultRequestHeaders.Clear();

//            var response = await _httpClient.GetAsync(uri);
//            Assert.True(response.IsSuccessStatusCode);

//            var content = await response.Content.ReadAsStringAsync();
//            var data = JsonConvert.DeserializeObject<ODataResponse<RestingHeartRate>>(content);
//            Assert.NotNull(data);
//            Assert.True(data.value.Count > 100);

//            var lastRestingHeartRate = data.value.OrderByDescending(x => x.DateTime).FirstOrDefault();
//            Assert.True(lastRestingHeartRate.Beats < 80);
//            Assert.True(lastRestingHeartRate.Beats > 40);
//        }
//    }
//}
