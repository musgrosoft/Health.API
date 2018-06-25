using Newtonsoft.Json;
using Repositories;
using Repositories.Health;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Xunit;
using Repositories.Models;
using HealthAPI.Acceptance.Tests.OData;

namespace HealthAPI.Acceptance.Tests
{
    public class ActivitySummariesAcceptanceTests
    {
        private HttpClient _httpClient;

        public ActivitySummariesAcceptanceTests()
        {
            _httpClient = new HttpClient();
        }


        [Fact]
        public async Task ShouldGetActivitySummaries()
        {
            var config = new Config();
            var healthContext = new HealthContext(config);
            var healthRepository = new HealthRepository(healthContext);

            var allActivitySummaries = healthRepository.GetAllActivitySummaries();
            var countDailyActivities = allActivitySummaries.Count();

            var activitySummaries = await GetActivitySummariesFromAPI();

            Assert.Equal(countDailyActivities, activitySummaries.Count());

            //var lastStepCount = data.value.OrderByDescending(x => x.DateTime).FirstOrDefault();
            //Assert.True(lastStepCount.Count < 30000);
            //Assert.True(lastStepCount.Count > 10);

            var activitySummary = new ActivitySummary
            {
                DateTime = new DateTime(1970, 1, 1),
                FairlyActiveMinutes =123
            };

            healthRepository.Upsert(activitySummary);

            activitySummaries = await GetActivitySummariesFromAPI();
            Assert.Equal(countDailyActivities+1, activitySummaries.Count());

            healthRepository.Delete(activitySummary);

            activitySummaries = await GetActivitySummariesFromAPI();
            Assert.Equal(countDailyActivities, activitySummaries.Count());
        }

        private async Task<IEnumerable<ActivitySummary>> GetActivitySummariesFromAPI()
        {
            var uri = "http://musgrosoft-health-api.azurewebsites.net/odata/ActivitySummaries";
            _httpClient.DefaultRequestHeaders.Clear();

            var response = await _httpClient.GetAsync(uri);
            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ODataResponse<ActivitySummary>>(content);
            Assert.NotNull(data);

            return data.value;
        }

        
//'http://musgrosoft-health-api.azurewebsites.net/odata/ActivitySummaries/GroupByMonth';
//'http://musgrosoft-health-api.azurewebsites.net/odata/ActivitySummaries/GroupByWeek';


    }
}
