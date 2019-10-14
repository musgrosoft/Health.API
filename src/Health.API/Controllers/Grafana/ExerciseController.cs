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


    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private enum GrafanaExerciseTargets
        {
            Exercise_Daily_TotalMinutes,
            Exercise_Weekly_TotalMinutes,
            Exercise_Monthly_TotalMinutes
        }

        private readonly IHealthService _healthService;
        private readonly ISheetsService _sheetsService;

        public ExerciseController(IHealthService healthService, ISheetsService sheetsService)
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
                    Enum.GetNames(typeof(GrafanaExerciseTargets)).ToList()
                );
        }

        [HttpGet, HttpPost]
        [Route("query")]
        public IActionResult Query([FromBody] GrafanaRequest grafanaRequest)
        {
            var responses = new List<QueryResponse>();
            
            foreach(var target in grafanaRequest.targets)
            {
                var t = Enum.Parse(typeof(GrafanaExerciseTargets), target.target);

                var qr = GetQueryResponse((GrafanaExerciseTargets)t);

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

        private List<Exercise> _rawExercises;
        private List<Exercise> GetRawExercisese()
        {
            if(_rawExercises == null)
            {
                _rawExercises = _sheetsService.GetExercises().OrderBy(x=>x.CreatedDate).ToList();
            }

            return _rawExercises;
        }

        private List<Exercise> GetDailyExercises()
        {
            return GetRawExercisese()
                        .GroupBy(x => x.CreatedDate)
                        .Select(x => new Exercise
                        {
                            CreatedDate = x.Key,
                            TotalSeconds = x.Sum(y => y.TotalSeconds / 60),
                            Metres = x.Sum(y => y.Metres),
                            Description = "Daily Sum"
                        }).ToList();
        }

        private IEnumerable<Exercise> GetWeeklyExercises()
        {
            return GetRawExercisese()
                        .GroupBy(x => x.CreatedDate.StartOfWeek(DayOfWeek.Monday))
                        .Select(x => new Exercise
                        {
                            CreatedDate = x.Key,
                            TotalSeconds = x.Sum(y => y.TotalSeconds / 60),
                            Metres = x.Sum(y => y.Metres),
                            Description = "Weekly Sum"
                        }).ToList();
        }

        private IEnumerable<Exercise> GetMonthlyExercises()
        {
            return GetRawExercisese()
               .GroupBy(x => new DateTime(x.CreatedDate.Year, x.CreatedDate.Month, 1))
               .Select(x => new Exercise
               {
                   CreatedDate = x.Key,
                   TotalSeconds = x.Sum(y => y.TotalSeconds / 60),
                   Metres = x.Sum(y => y.Metres),
                   Description = "Monthly Sum"
               }).ToList();
        }

        private QueryResponse GetQueryResponse(GrafanaExerciseTargets tt)
        {
            switch (tt)
            {

                case GrafanaExerciseTargets.Exercise_Daily_TotalMinutes:
                    return
                    new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = GetDailyExercises()
                            .Select(x => new double?[] { x.TotalSeconds, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                            .ToList()

                    };

                case GrafanaExerciseTargets.Exercise_Weekly_TotalMinutes:
                    return
                    new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = GetWeeklyExercises()
                            .Select(x => new double?[] { x.TotalSeconds, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                            .ToList()

                    };


                case GrafanaExerciseTargets.Exercise_Monthly_TotalMinutes:
                    return
                    new QueryResponse
                    {
                        Target = tt.ToString(),
                        Datapoints = GetMonthlyExercises()
                            .Select(x => new double?[] { x.TotalSeconds, x.CreatedDate.ToUnixTimeMillisecondsFromDate() })
                            .ToList()

                    };



                default:
                    return null;


            }

        }


    }

}