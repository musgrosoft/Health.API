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
using Withings;

namespace Health.API.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class GrafanaBloodPressureController : ControllerBase
    {
        private enum TTarget
        {

            BloodPressureSystolic,
            BloodPressureSystolicMovingAverage,

            BloodPressureDiastolic,
            BloodPressureDiastolicMovingAverage,


        }

        private readonly IWithingsService _withingsService;

        public GrafanaBloodPressureController(IWithingsService withingsService)
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

        private List<BloodPressure> _rawBPs;
        private async Task<List<BloodPressure>> GetRawBPs()
        {
            if (_rawBPs == null)
            {
                _rawBPs = (await _withingsService.GetBloodPressures(new DateTime(2010,1,1))).ToList().OrderBy(x => x.CreatedDate).ToList();
            }

            return _rawBPs;
        }

        // private List<Weight> _orderedWeights;
        private async Task<List<BloodPressure>> GetAllBPsAggregatedByDay()
        {
            //if (_orderedWeights == null)
            //{
                return (await GetRawBPs())
                    .GroupBy(x=>x.CreatedDate.Date)
                    .Select(x=> new BloodPressure
                    {
                        CreatedDate = x.Key,
                        Diastolic = x.Average(y => y.Diastolic),
                        Systolic = x.Average(y=>y.Systolic) ,
                        
                    })
                    .OrderBy(x => x.CreatedDate).ToList();
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


                case TTarget.BloodPressureSystolic:

                    return new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = (await GetAllBPsAggregatedByDay())
                                .Select(x => new double?[] { x.Systolic, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                                .ToList()
                    };

                case TTarget.BloodPressureSystolicMovingAverage:
                    return
                    new QueryResponse
                    {
                        //var averaged = mySeries.Windowed(period).Select(window => window.Average(keyValuePair => keyValuePair.Value));

                        Target = tt.ToString(),
                        Datapoints = (await GetAllBPsAggregatedByDay())
                                .WindowRight(10)
                                .Select(window => new double?[] { window.Average(x => x.Systolic), window.Max(x => x.CreatedDate).ToUnixTimeMillisecondsFromDate() })
                                //                                .Select( x => new double?[] { x.Kg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() } )
                                .ToList()
                    };

                case TTarget.BloodPressureDiastolic:

                    return new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = (await GetAllBPsAggregatedByDay())
                            .Select(x => new double?[] { x.Diastolic, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                            .ToList()
                    };

                case TTarget.BloodPressureDiastolicMovingAverage:
                    return
                        new QueryResponse
                        {
                            //var averaged = mySeries.Windowed(period).Select(window => window.Average(keyValuePair => keyValuePair.Value));

                            Target = tt.ToString(),
                            Datapoints = (await GetAllBPsAggregatedByDay())
                                .WindowRight(10)
                                .Select(window => new double?[] { window.Average(x => x.Diastolic), window.Max(x => x.CreatedDate).ToUnixTimeMillisecondsFromDate() })
                                //                                .Select( x => new double?[] { x.Kg, x.CreatedDate.ToUnixTimeMillisecondsFromDate() } )
                                .ToList()
                        };

               


                default:
                    return null;


            }

        }

    }


}