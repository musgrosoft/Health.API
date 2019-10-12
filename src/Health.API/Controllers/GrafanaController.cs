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
using Repositories.Health.Models;
using GoogleSheets;

namespace Health.API.Controllers
{
    public enum TTarget
    {
        Sleeps,

        WeightKg,
        WeightKgMovingAverage,
        WeightPercentageFat,
        WeightPercentageFatMovingAverage,
        WeightFatKg,
        WeightFatKgMovingAverage,
        WeightLeanKg,
        WeightLeanKgMovingAverage,

        Exercise_Daily_TotalSeconds
    }

    [Route("api/[controller]")]
    [ApiController]
    public class GrafanaController : ControllerBase
    {
        private readonly IHealthService _healthService;
        private readonly ISheetsService _sheetsService;

        public GrafanaController(IHealthService healthService, ISheetsService sheetsService)
        {
            _healthService = healthService;
            _sheetsService = sheetsService;
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
                    Enum.GetNames(typeof(TTarget)).ToList()
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

       // private List<Weight> _orderedWeights;
        private List<Weight> GetAllWeightsAggregatedByDay()
        {
            //if (_orderedWeights == null)
            //{
                return _healthService.GetLatestWeights(20000)
                    .GroupBy(x=>x.CreatedDate.Date)
                    .Select(x=> new Weight
                    {
                        CreatedDate = x.Key,
                        Kg = x.Average(y => y.Kg),
                        FatRatioPercentage = x.Average(y=>y.FatRatioPercentage) ,
                    })
                    .OrderBy(x => x.CreatedDate).ToList();
            //}

            //return _orderedWeights;

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

                case TTarget.Exercise_Daily_TotalSeconds:

                    //var gService = new GoogleSheets.SheetsService()
                    return
                    new QueryResponse
                    {


                        Target = tt.ToString(),
                        Datapoints = _sheetsService.GetExercises()
                        .GroupBy(x=>x.CreatedDate)
                        .Select(x=> new Exercise
                        {
                            CreatedDate = x.Key,
                            TotalSeconds = x.Sum(y=>y.TotalSeconds),
                            Metres = x.Sum(y=>y.Metres),
                            Description = "Daily Sum"
                        })



                        //_healthService.GetLatestExercises(20000)
                            .OrderBy(x => x.CreatedDate)
                            .Select(x => new double?[] { x.TotalSeconds, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                            .ToList()

                    };

                case TTarget.WeightKg:

                    return new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = GetAllWeightsAggregatedByDay()
                                .Select(x => new double?[] { x.Kg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                                .ToList()
                    };

                case TTarget.WeightKgMovingAverage:
                    return
                    new QueryResponse
                    {
                        //var averaged = mySeries.Windowed(period).Select(window => window.Average(keyValuePair => keyValuePair.Value));

                        Target = tt.ToString(),
                        Datapoints = GetAllWeightsAggregatedByDay()
                                .WindowRight(10)
                                .Select(window => new double?[] { window.Average(x => x.Kg), window.Max(x => x.CreatedDate).ToUnixTimeMillisecondsFromDate() })
                                //                                .Select( x => new double?[] { x.Kg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() } )
                                .ToList()
                    };

                case TTarget.WeightPercentageFat:

                    return new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = GetAllWeightsAggregatedByDay()
                            .Select(x => new double?[] { x.FatRatioPercentage, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                            .ToList()
                    };

                case TTarget.WeightPercentageFatMovingAverage:
                    return
                        new QueryResponse
                        {
                            //var averaged = mySeries.Windowed(period).Select(window => window.Average(keyValuePair => keyValuePair.Value));

                            Target = tt.ToString(),
                            Datapoints = GetAllWeightsAggregatedByDay()
                                .WindowRight(10)
                                .Select(window => new double?[] { window.Average(x => x.FatRatioPercentage), window.Max(x => x.CreatedDate).ToUnixTimeMillisecondsFromDate() })
                                //                                .Select( x => new double?[] { x.Kg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() } )
                                .ToList()
                        };

                case TTarget.WeightFatKg:

                    return new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = GetAllWeightsAggregatedByDay()
                            .Select(x => new double?[] { x.FatKg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                            .ToList()
                    };

                case TTarget.WeightFatKgMovingAverage:
                    return
                        new QueryResponse
                        {
                            //var averaged = mySeries.Windowed(period).Select(window => window.Average(keyValuePair => keyValuePair.Value));

                            Target = tt.ToString(),
                            Datapoints = GetAllWeightsAggregatedByDay()
                                .WindowRight(10)
                                .Select(window => new double?[] { window.Average(x => x.FatKg), window.Max(x => x.CreatedDate).ToUnixTimeMillisecondsFromDate() })
                                //                                .Select( x => new double?[] { x.Kg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() } )
                                .ToList()
                        };

                case TTarget.WeightLeanKg:

                    return new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = GetAllWeightsAggregatedByDay()
                            .Select(x => new double?[] { x.LeanKg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                            .ToList()
                    };

                case TTarget.WeightLeanKgMovingAverage:
                    return
                        new QueryResponse
                        {
                            //var averaged = mySeries.Windowed(period).Select(window => window.Average(keyValuePair => keyValuePair.Value));

                            Target = tt.ToString(),
                            Datapoints = GetAllWeightsAggregatedByDay()
                                .WindowRight(10)
                                .Select(window => new double?[] { window.Average(x => x.LeanKg), window.Max(x => x.CreatedDate).ToUnixTimeMillisecondsFromDate() })
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