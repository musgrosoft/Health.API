using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HealthAPI.Acceptance.Tests.OData;
using Newtonsoft.Json;
using Xunit;
using Repositories.Models;

namespace HealthAPI.Acceptance.Tests
{
    public class AlcoholIntakesAcceptanceTests
    {
        private HttpClient _httpClient;

        public AlcoholIntakesAcceptanceTests()
        {
            _httpClient = new HttpClient();
        }

        [Fact]
        public async Task ShouldGetAlcoholIntakes()
        {
            var uri = "http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes";
            _httpClient.DefaultRequestHeaders.Clear();

            var response = await _httpClient.GetAsync(uri);
            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ODataResponse<AlcoholIntake>>(content);
            Assert.NotNull(data);
//            Assert.True(data.value.Count > 100);

            var lastAlcoholIntake = data.value.OrderByDescending(x => x.DateTime).FirstOrDefault();
        //    Assert.True(lastAlcoholIntake.Units < 100);
        //    Assert.True(lastAlcoholIntake.Units > -1);
        }

//'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes?$filter=year(DateTime)%20eq%202016';
//'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes?$filter=year(DateTime)%20eq%202017';
//'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes?$filter=year(DateTime)%20eq%202018';
//'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes/GroupByMonth';
//'http://musgrosoft-health-api.azurewebsites.net/odata/AlcoholIntakes/GroupByWeek';

        
    }
}
