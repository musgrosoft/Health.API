using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Health;
using Utils;
using MoreLinq;
using Repositories;

namespace Health.API.Controllers
{
    public enum TTarget
    {
        Sleeps,
        Weights,
        WeightsMovingAverage
    }

    [Route("api/[controller]")]
    [ApiController]
    public class GrafanaController : ControllerBase
    {
        private readonly IHealthService _healthService;

        public GrafanaController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet, HttpPost]
        [Route("search")]
        public IActionResult Search()
        {
            return Ok(
                new List<string>
                {
                    "sleeps",
                    "weights",
                    "weightsMovingAverage"
                }
                );
        }

        [HttpGet, HttpPost]
        [Route("query")]
        public IActionResult Query([FromBody] GrafanaRequest grafanaRequest)
        {
            var responses = new List<QueryResponse>();
            
            foreach(var target in grafanaRequest.targets)
            {
                var t = Enum.Parse(typeof(TTarget), target.target);

                var qr = GetQueryResponse((TTarget)t);

                responses.Add(qr);


            }

            if(responses.Any())
            {
                return Ok(responses);
            }
            else {
                return Ok("no matching target " + grafanaRequest.ToString());
            }


            
        }

        private QueryResponse GetQueryResponse(TTarget tt)
        {
            switch (tt)
            {
                case TTarget.Sleeps:
                        return
                        new QueryResponse
                        {
                            Target = tt.ToString(),
                            Datapoints = _healthService.GetLatestSleeps(20000)
                                .OrderBy(x => x.DateOfSleep)
                                .Select(x => new double?[] { x.AsleepMinutes, x.DateOfSleep.ToUnixTimeMillisecondsFromDate() })
                                .ToList()

                        };

                case TTarget.Weights:

                    return new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = _healthService.GetLatestWeights(20000)
                                .OrderBy(x => x.CreatedDate)
                                .Select(x => new double?[] { x.Kg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                                .ToList()
                    };

                case TTarget.WeightsMovingAverage:
                    return
                    new QueryResponse
                    {
                        //var averaged = mySeries.Windowed(period).Select(window => window.Average(keyValuePair => keyValuePair.Value));

                        Target = tt.ToString(),
                        Datapoints = _healthService.GetLatestWeights(20000)
                                .OrderBy(x => x.CreatedDate)
                                .WindowRight(10)
                                .Select(window => new double?[] { window.Average(x => x.Kg), window.Max(x => x.CreatedDate).ToUnixTimeMillisecondsFromDate() })
                                //                                .Select( x => new double?[] { x.Kg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() } )
                                .ToList()
                    };

                default:
                    return null;


            }

        }

    }

    public class QueryResponse
    {
        public string Target { get; set; }
        public List<double?[]> Datapoints { get; set; }
    }

//    public class Datapoint
//    {
//        public double Value { get; set; }
//        public double UnixTimestamp { get; set; }
//    }

}