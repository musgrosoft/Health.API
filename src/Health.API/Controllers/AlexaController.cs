using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Health;
using Services.OAuth;

namespace HealthAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Alexa")]
    public class AlexaController : Controller
    {
        private readonly IHealthService _healthService;
        private readonly ITokenService _tokenService;

        public AlexaController(IHealthService healthService, ITokenService tokenService)
        {
            _healthService = healthService;
            _tokenService = tokenService;
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
        public async Task<IActionResult> FlashBriefing()
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

            var flashBriefings = new List<FlashBriefing>();

            var technicalErrorFlashBriefing = await GetTechnicalErrorFlashBriefing();

            if (technicalErrorFlashBriefing != null)
            {
                flashBriefings.Add(technicalErrorFlashBriefing);
            }

            var oldDataFlashBriefing = GetOldDataFlashBriefing();

            if (oldDataFlashBriefing != null)
            {
                flashBriefings.Add(oldDataFlashBriefing);
            }

//            var hitTargetsFlashBriefing = GetHitTargetsFlashBriefing();


            //Content-Type: application/json
            return Ok(flashBriefings);
        }

//        private FlashBriefing GetHitTargetsFlashBriefing()
//        {
//            var latestWeights = _healthService.GetLatestWeights();
//            var latestBloodpressures = _healthService.GetLatestBloodPressures();
//            var latestRestingHeartRates = _healthService.GetLatestRestingHeartRates();
//
//            var averageWeight = latestWeights.Average(x => x.Kg);
//            var targetWeight 
//        }

        private FlashBriefing GetOldDataFlashBriefing()
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
            
            var messages = "";

            if (daysOldWeight > 4)
            {
                messages += $"Weight was last updated {daysOldWeightExpression}. ";
            }

            if (daysOldBloodpressure > 4)
            {
                messages += $"Blood pressure was last updated {daysOldBloodpressureExpression}. ";
            }

            if (daysOldRestingHeartRate > 2)
            {
                messages += $"Resting heart rate was last updated {daysOldRestingHeartRateExpression}. ";
            }

            if (daysOldDrinkDate > 3)
            {
                messages += $"Drinks were last updated {daysOldDrinkDateExpression}. ";
            }

            if (daysOldExerciseDate > 3)
            {
                messages += $"Exercise was last updated {daysOldExerciseDateExpression}. ";
            }

            if (string.IsNullOrWhiteSpace(messages))
            {
                return null;
            }
            else
            {
                return new FlashBriefing
                {
                    uid = "OLD DATA",
                    updateDate = DateTime.Now.AddMinutes(2).ToString("yyyy-MM-ddTHH:mm:ss.0Z"),
                    titleText = "Old Date",
                    mainText = $"There is some old data. {messages}",
                    redirectionUrl = "https://www.amazon.com"
                };
            }
        }

        private async Task<FlashBriefing> GetTechnicalErrorFlashBriefing()
        {
            var fitbitRefreshToken = await _tokenService.GetFitbitRefreshToken();
            var withingsRefreshToken = await _tokenService.GetWithingsRefreshToken();

            var messages = "";

            if (string.IsNullOrWhiteSpace(fitbitRefreshToken))
            {
                messages += "Fitbit Refresh Token is empty.";
            }

            if (string.IsNullOrWhiteSpace(withingsRefreshToken))
            {
                messages += "Withings Refresh Token is empty.";
            }


            if (string.IsNullOrWhiteSpace(messages))
            {
                return null;
            }
            else
            {
                return new FlashBriefing
                {
                    uid = "TECHNICAL ERRORS",
                    updateDate = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-ddTHH:mm:ss.0Z"), 
                    titleText = "Technical Errors",
                    mainText = $"There are technical errors. {messages}",
                    redirectionUrl = "https://www.amazon.com"
                };
            }


        }
    }
}