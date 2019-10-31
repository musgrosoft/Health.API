using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Health.API.Controllers.Grafana;
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
        private enum GrafanaTarget
        {
            WeightKg,
            WeightKgMovingAverage,
            WeightPercentageFat,
            WeightPercentageFatMovingAverage,
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
                    Enum.GetNames(typeof(GrafanaTarget)).ToList()
                );
        }

        [HttpGet, HttpPost]
        [Route("query")]
        public async Task<IActionResult> Query([FromBody] GrafanaRequest grafanaRequest)
        {
            var responses = new List<QueryResponse>();

            var startTime = grafanaRequest.startTime.ToDateFromUnixTimeMilliseconds();
            var endTime = grafanaRequest.endTime.ToDateFromUnixTimeMilliseconds();

            foreach (var target in grafanaRequest.targets)
            {
                var t = Enum.Parse(typeof(GrafanaTarget), target.target);

                var qr = await GetQueryResponse((GrafanaTarget)t, startTime, endTime);

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


        private List<GrafanaWeight> _grafanaWeights;
        private async Task<List<GrafanaWeight>> GetAllWeightsAggregatedByDay()
        {
            if (_grafanaWeights == null)
            {

                var startDate = new DateTime(2010, 1, 1);
                var endDate = DateTime.Now.Date;

                var rawWeights = (await _withingsService.GetWeights(new DateTime(2010, 1, 1))).ToList()
                    .OrderBy(x => x.CreatedDate).ToList();

                var dateRange = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                    .Select(offset => startDate.AddDays(offset));

                var weightsAggregatedByDay = rawWeights
                    .GroupBy(x => x.CreatedDate.Date)
                    .Select(x => new Weight
                    {
                        CreatedDate = x.Key,
                        Kg = x.Average(y => y.Kg),
                        FatRatioPercentage = x.Average(y => y.FatRatioPercentage),
                    })
                    .OrderBy(x => x.CreatedDate);


                var weightsOverDateRange = dateRange
                    .GroupJoin(weightsAggregatedByDay, d => d.Date, w => w.CreatedDate,
                        (d, w) => (w != null && w.Any()) ? w.Single() : new Weight {CreatedDate = d});

                _grafanaWeights = weightsOverDateRange
                    .WindowRight(10)
                    .Select(window =>
                        new GrafanaWeight
                        {
                            //Day = window.Max(x => x.CreatedDate),
                            Day = window.Last().CreatedDate,
                            MovingAverageKg = window.Average(x => x.Kg),
                            Kg = window.Last().Kg,
                            FatRatioPercentage = window.Last().FatRatioPercentage,
                            MovingAverageFatRatioPercentage = window.Average(x => x.FatRatioPercentage),

                        })
                    .ToList();

            }

            return _grafanaWeights;

        }

        private async Task<QueryResponse> GetQueryResponse(GrafanaTarget tt, DateTime startTime, DateTime endTime)
        {
            var timeFilteredWeights = (await GetAllWeightsAggregatedByDay());//.Where(x => x.Day.Between(startTime, endTime));

            switch (tt)
            {
                case GrafanaTarget.WeightKg:

                    return new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = timeFilteredWeights.Select(x => new double?[] { x.Kg, x.Day.ToUnixTimeMillisecondsFromDate() }).ToList()
                    };

                case GrafanaTarget.WeightKgMovingAverage:
                    return
                    new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = timeFilteredWeights.Select(x => new double?[] { x.MovingAverageKg, x.Day.ToUnixTimeMillisecondsFromDate() }).ToList()
                    };

                case GrafanaTarget.WeightPercentageFat:

                    return new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = timeFilteredWeights.Select(x => new double?[] { x.FatRatioPercentage, x.Day.ToUnixTimeMillisecondsFromDate() }).ToList()
                    };

                case GrafanaTarget.WeightPercentageFatMovingAverage:
                    return
                        new QueryResponse
                        {
                            Target = tt.ToString(),
                            Datapoints = timeFilteredWeights.Select(x => new double?[] { x.MovingAverageFatRatioPercentage, x.Day.ToUnixTimeMillisecondsFromDate() }).ToList()
                        };


                default:
                    return null;


            }

        }

    }


}