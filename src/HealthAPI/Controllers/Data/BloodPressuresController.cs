using System;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Services.Health;

namespace HealthAPI.Controllers.Data
{
    [Produces("application/json")]
    [Route("api/BloodPressures")]

    public class BloodPressuresController : Controller
    {
        private readonly IHealthService _healthService;

        public BloodPressuresController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Json(_healthService.GetAllBloodPressures());
        }

        [HttpGet]
        [Route("Migrate")]
        public IActionResult Migrate()
        {
            var bloodPressures = _healthService.GetAllBloodPressures();

            var elasticSearchUrl = Environment.GetEnvironmentVariable("ElasticSearchUrl");
            var node = new Uri(elasticSearchUrl);
            var config = new ConnectionConfiguration(node);
            var elasticSearchClient = new ElasticLowLevelClient(config);

            //var elasticsearchResponse = await _elasticSearchClient
            //    .CreatePostAsync<BytesResponse>(elasticSearchIndex,
            //        ElasticsearchIndexType,
            //        Guid.NewGuid().ToString(), PostData.Serializable(data));

            //elasticSearchClient.IndicesDelete<BytesResponse>("bloodpressures");

            foreach (var bloodPressure in bloodPressures)
            {
                var elasticsearchResponse = elasticSearchClient
                    .Index<BytesResponse>(
                        "bloodpressures",
                        "bloodpressure",
                        bloodPressure.CreatedDate.ToString(),
                        PostData.Serializable(bloodPressure),
                        null
                    );
            }



            //if (!elasticsearchResponse.Success)
            //    _exceptionlessClientWrapper.SubmitException(new Exception(
            //        $"Log to elastic search failed to {ConfigurationAdapter.ElasticSearchUrl}",
            //        elasticsearchResponse.OriginalException));



            return Ok();
        }
    }
}