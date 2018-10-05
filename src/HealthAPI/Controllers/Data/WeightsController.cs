using System;
using System.Collections.Generic;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Services.Health;

namespace HealthAPI.Controllers.Data
{
    [Produces("application/json")]
    [Route("api/Weights")]
    public class WeightsController : Controller
    {
        private readonly IHealthService _healthService;

        public WeightsController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<Weight>), 200)]
        public IActionResult Get()
        {
            return Json(_healthService.GetAllWeights());
        }

        [HttpGet]
        [Route("Migrate")]
        public IActionResult Migrate()
        {
            var weights = _healthService.GetAllWeights();

            var elasticSearchUrl = Environment.GetEnvironmentVariable("ElasticSearchUrl");
            var node = new Uri(elasticSearchUrl);
            var config = new ConnectionConfiguration(node);
            var elasticSearchClient = new ElasticLowLevelClient(config);

            //var elasticsearchResponse = await _elasticSearchClient
            //    .CreatePostAsync<BytesResponse>(elasticSearchIndex,
            //        ElasticsearchIndexType,
            //        Guid.NewGuid().ToString(), PostData.Serializable(data));

            elasticSearchClient.IndicesDelete<BytesResponse>("weights");

            foreach (var weight in weights)
            {
                var elasticsearchResponse = elasticSearchClient
                    .Index<BytesResponse>(
                        "weights",
                        "weight",
                        weight.CreatedDate.ToString(),
                        PostData.Serializable(weight),
                        null
                    );
            }



            //if (!elasticsearchResponse.Success)
            //    _exceptionlessClientWrapper.SubmitException(new Exception(
            //        $"Log to elastic search failed to {ConfigurationAdapter.ElasticSearchUrl}",
            //        elasticsearchResponse.OriginalException));



            return Ok();
        }

        //[HttpGet]
        //public IList<Weight> Get()
        //{
        //    return _healthService.GetAllWeights();
        //}

    }
}