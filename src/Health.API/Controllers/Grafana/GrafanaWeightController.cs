using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Utils;
using MoreLinq;
using Repositories.Health.Models;
using Withings;

namespace Health.API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class GrafanaWeightController : ControllerBase
    {
        private enum TTarget
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

        }

        private readonly IWithingsService _withingsService;

        public GrafanaWeightController(IWithingsService withingsService)
        {
            _withingsService = withingsService;
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
        public async Task<IActionResult> Query([FromBody] GrafanaRequest grafanaRequest)
        {
            var responses = new List<QueryResponse>();
            
            foreach(var target in grafanaRequest.targets)
            {
                var t = Enum.Parse(typeof(TTarget), target.target);

                var qr = await GetQueryResponse((TTarget)t);

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

        private List<Weight> _rawWeights;
        private async Task<List<Weight>> GetRawWeights()
        {
            if (_rawWeights == null)
            {
                _rawWeights = (await _withingsService.GetWeights(new DateTime(2010,1,1))).ToList().OrderBy(x => x.CreatedDate).ToList();
            }

            return _rawWeights;
        }

        // private List<Weight> _orderedWeights;
        private async Task<List<Weight>> GetAllWeightsAggregatedByDay()
        {
            //if (_orderedWeights == null)
            //{

            var startDate = new DateTime(2010, 1, 1);
            var endDate = DateTime.Now.Date;

            var dateRange = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                                            .Select(offset => startDate.AddDays(offset));

            var weightsAggregatedByDay = (await GetRawWeights())
                    .GroupBy(x => x.CreatedDate.Date)
                    .Select(x => new Weight
                    {
                        CreatedDate = x.Key,
                        Kg = x.Average(y => y.Kg),
                        FatRatioPercentage = x.Average(y => y.FatRatioPercentage),
                    })
                    .OrderBy(x => x.CreatedDate).ToList();


            return dateRange
                .GroupJoin(weightsAggregatedByDay, d => d.Date, w => w.CreatedDate, (d,w) => w != null ? w.Single() : new Weight { })
                .ToList();

            //}

            //return _orderedWeights;

        }

        private async Task<QueryResponse> GetQueryResponse(TTarget tt)
        {
            switch (tt)
            {
//                case TTarget.Sleeps:
//                        return
//                        new QueryResponse
//                        {
//                            Target = tt.ToString(),
//                            Datapoints = _withingsService.GetLatestSleeps(20000)
//                                .OrderBy(x => x.DateOfSleep)
//                                .Select(x => new double?[] { x.AsleepMinutes, x.DateOfSleep.ToUnixTimeMillisecondsFromDate() })
//                                .ToList()

                        //};


                case TTarget.WeightKg:

                    return new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = (await GetAllWeightsAggregatedByDay())
                                .Select(x => new double?[] { x.Kg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                                .ToList()
                    };

                case TTarget.WeightKgMovingAverage:
                    return
                    new QueryResponse
                    {
                        //var averaged = mySeries.Windowed(period).Select(window => window.Average(keyValuePair => keyValuePair.Value));

                        Target = tt.ToString(),
                        Datapoints = (await GetAllWeightsAggregatedByDay())
                                .WindowLeft(10)
                                .Select(window => new double?[] { window.Average(x => x.Kg), window.Max(x => x.CreatedDate.ToUnixTimeMillisecondsFromDate()) })
                                //                                .Select( x => new double?[] { x.Kg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() } )
                                .ToList()
                    };

                case TTarget.WeightPercentageFat:

                    return new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = (await GetAllWeightsAggregatedByDay())
                            .Select(x => new double?[] { x.FatRatioPercentage, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                            .ToList()
                    };

                case TTarget.WeightPercentageFatMovingAverage:
                    return
                        new QueryResponse
                        {
                            //var averaged = mySeries.Windowed(period).Select(window => window.Average(keyValuePair => keyValuePair.Value));

                            Target = tt.ToString(),
                            Datapoints = (await GetAllWeightsAggregatedByDay())
                                .WindowRight(10)
                                .Select(window => new double?[] { window.Average(x => x.FatRatioPercentage), window.Max(x => x.CreatedDate).ToUnixTimeMillisecondsFromDate() })
                                //                                .Select( x => new double?[] { x.Kg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() } )
                                .ToList()
                        };

                case TTarget.WeightFatKg:

                    return new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = (await GetAllWeightsAggregatedByDay())
                            .Select(x => new double?[] { x.FatKg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                            .ToList()
                    };

                case TTarget.WeightFatKgMovingAverage:
                    return
                        new QueryResponse
                        {
                            //var averaged = mySeries.Windowed(period).Select(window => window.Average(keyValuePair => keyValuePair.Value));

                            Target = tt.ToString(),
                            Datapoints = (await GetAllWeightsAggregatedByDay())
                                .WindowRight(10)
                                .Select(window => new double?[] { window.Average(x => x.FatKg), window.Max(x => x.CreatedDate).ToUnixTimeMillisecondsFromDate() })
                                //                                .Select( x => new double?[] { x.Kg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() } )
                                .ToList()
                        };

                case TTarget.WeightLeanKg:

                    return new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = (await GetAllWeightsAggregatedByDay())
                            .Select(x => new double?[] { x.LeanKg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                            .ToList()
                    };

                case TTarget.WeightLeanKgMovingAverage:
                    return
                        new QueryResponse
                        {
                            //var averaged = mySeries.Windowed(period).Select(window => window.Average(keyValuePair => keyValuePair.Value));

                            Target = tt.ToString(),
                            Datapoints = (await GetAllWeightsAggregatedByDay())
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


}