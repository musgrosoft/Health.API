using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Services.Health;

namespace HealthAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Alexa")]
    public class AlexaController : Controller
    {
        private readonly IHealthService _healthService;

        public AlexaController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        private string PrettifyDaysOld(double daysOld)
        {
            switch (daysOld)
            {
                case 0:
                    return "today";
                case 1:
                    return "yesterday";
                default:
                    return $"from {daysOld} days ago";
            }
        }

        private double DaysOld(DateTime dateTime)
        {
            return (DateTime.Now.Date - dateTime.Date).TotalDays;
        }

        // GET
        [HttpGet]
        [Route("FlashBriefing")]
        public IActionResult FlashBriefing()
        {
            var latestWeightDate = _healthService.GetLatestWeightDate(DateTime.MinValue);
            var daysOldWeight = DaysOld(latestWeightDate);
            var daysOldWeightExpression = PrettifyDaysOld(daysOldWeight);
            
            var latestBloodPressureDate = _healthService.GetLatestBloodPressureDate(DateTime.MinValue);
            var daysOldBloodpressure = DaysOld(latestBloodPressureDate);
            var daysOldBloodpressureExpression = PrettifyDaysOld(daysOldBloodpressure);

            var latestRestingHeartRate = _healthService.GetLatestRestingHeartRateDate(DateTime.MinValue);
            var daysOldRestingHeartRate = DaysOld(latestRestingHeartRate);
            var daysOldRestingHeartRateExpression = PrettifyDaysOld(daysOldRestingHeartRate);

            var latestDrinkDate = _healthService.GetLatestDrinkDate();
            var daysOldDrinkDate = DaysOld(latestDrinkDate);
            var daysOldDrinkDateExpression = PrettifyDaysOld(daysOldDrinkDate);

            var latestExerciseDate = _healthService.GetLatestExerciseDate(DateTime.MinValue);
            var daysOldExerciseDate = DaysOld(latestExerciseDate);
            var daysOldExerciseDateExpression = PrettifyDaysOld(daysOldExerciseDate);
            
            var latestWeights = _healthService.GetLatestWeights();
            var latestBloodpressures = _healthService.GetLatestBloodPressures();
            var latestRestingHeartRates = _healthService.GetLatestRestingHeartRates();



            var flashBriefings = new List<FlashBriefing>
                {

                    new FlashBriefing
                    {
                        uid = "1_AGE_OF_DATA",
                        updateDate = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-ddTHH:mm:ss.0Z"), //"2019-06-10T22:34:51.0Z" ,
                        titleText = "Latest recordings",
                        mainText = $"Latest Weight recorded {daysOldWeightExpression}. " +
                                   $"Latest Blood pressure recorded {daysOldBloodpressureExpression}. " +
                                   $"Latest Resting Heart Rate recorded {daysOldRestingHeartRateExpression}. " +
                                   $"Latest Drink recorded {daysOldDrinkDateExpression}. " +
                                   $"Latest Exercise recorded {daysOldExerciseDateExpression}. ",
                        redirectionUrl = "https://www.amazon.com"
                    },

                new FlashBriefing
                {
                    uid = "2_DATA",
                    updateDate = DateTime.Now.AddMinutes(2).ToString("yyyy-MM-ddTHH:mm:ss.0Z"), //"2019-06-10T22:34:51.0Z" ,
                    titleText = "Data",
                    mainText = $"Your Weight is {latestWeights.Average(x=>x.Kg).Value:0.#} kg. " +
                               $"Your blood pressure is {latestBloodpressures.Average(x=>x.Systolic).Value:N0} over {latestBloodpressures.Average(x=>x.Diastolic).Value:N0}. " +
                               $"Your resting heart rate is {latestRestingHeartRates.Average(x=>x.Beats):N0}. ",
                    redirectionUrl = "https://www.amazon.com"
                },
//                new FlashBriefing
//                {
//                    uid = "3_TARGETS",
//                    updateDate = DateTime.Now.AddMinutes(3).ToString("yyyy-MM-ddTHH:mm:ss.0Z"), //"2019-06-10T22:34:51.0Z" ,
//                    titleText = "Targets",
//                    mainText = $"This is some unique content. ",
//                    redirectionUrl = "https://www.amazon.com"
//                }

                };


            //Content-Type: application/json
            return Ok(flashBriefings);
        }
    }
}