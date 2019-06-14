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

//        private string PrettifyDaysOld(double daysOld)
//        {
//            switch (daysOld)
//            {
//                case 0:
//                    return "today";
//                case 1:
//                    return "yesterday";
//                default:
//                    return $"from {daysOld} days ago";
//            }
//        }

        private double DaysOld(DateTime dateTime)
        {
            return (DateTime.Now.Date - dateTime.Date).TotalDays;
        }

        // GET
        [HttpGet]
        [Route("FlashBriefing")]
        public async Task<IActionResult> FlashBriefing()
        {
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

            var targetsFlashBriefing = GetTargetsFlashBriefing();

            flashBriefings.AddRange(targetsFlashBriefing);
            
            //Content-Type: application/json
            return Ok(flashBriefings);
        }

        private List<FlashBriefing> GetTargetsFlashBriefing()
        {
            var latestWeights = _healthService.GetLatestWeights();
            var latestBloodpressures = _healthService.GetLatestBloodPressures();
            var latestRestingHeartRates = _healthService.GetLatestRestingHeartRates();

            var averageWeight = latestWeights.Average(x => x.Kg);

            var averageSystolic = latestBloodpressures.Average(x => x.Systolic);
            var averageDiastolic = latestBloodpressures.Average(x => x.Diastolic);



            //WHEN CalendarDate >= '2019/01/01' THEN 86    - ((3.000/365) * (DATEDIFF(day , '2019/01/01' , CalendarDate)))
            var targetWeight = 86 - ((DateTime.Now - new DateTime(2019, 1, 1)).Days * (3 / 365));

            var targetSystolic = 120;
            var targetDiastolic = 80;

            var hitMessages = "";
            var missedMessages = "";

            if (averageWeight < targetWeight)
            {
                hitMessages += $"Hitting weight target, you are {targetWeight-averageWeight} kilograms below target. ";
            }
            else
            {
                missedMessages += $"Missed weight target, you are {averageWeight - targetWeight} kilograms above target. ";
            }

            if (averageSystolic < targetSystolic)
            {
                hitMessages += $"Hitting systolic blood pressure target, you are {targetSystolic - averageSystolic} mmHg below target. ";
            }
            else
            {
                missedMessages += $"Missed systolic blood pressure target, you are {averageSystolic - targetSystolic} mmHg above target. ";
            }

            if (averageDiastolic < targetDiastolic)
            {
                hitMessages += $"Hitting diastolic blood pressure target, you are {targetDiastolic - averageDiastolic} mmHg below target. ";
            }
            else
            {
                missedMessages += $"Missing diastolic blood pressure target, you are {averageDiastolic - targetDiastolic} mmHg below target. ";
            }

            var briefings = new List<FlashBriefing>();

            if (!string.IsNullOrWhiteSpace(hitMessages))
            {
                briefings.Add(

                    new FlashBriefing
                    {
                        uid = "HIT TARGETS",
                        updateDate = DateTime.Now.AddMinutes(3).ToString("yyyy-MM-ddTHH:mm:ss.0Z"),
                        titleText = "Hit Targets",
                        mainText = $"You have hit these targets. {hitMessages}",
                        redirectionUrl = "https://www.amazon.com"
                    }

                  );
            }

            if (!string.IsNullOrWhiteSpace(hitMessages))
            {
                briefings.Add(

                    new FlashBriefing
                    {
                        uid = "MISSED TARGETS",
                        updateDate = DateTime.Now.AddMinutes(3).ToString("yyyy-MM-ddTHH:mm:ss.0Z"),
                        titleText = "Missed Targets",
                        mainText = $"You have missed these targets. {missedMessages}",
                        redirectionUrl = "https://www.amazon.com"
                    }

                );
            }

            return briefings;

        }

        private FlashBriefing GetOldDataFlashBriefing()
        {
            var latestWeightDate = _healthService.GetLatestWeightDate(DateTime.MinValue);
            var daysOldWeight = DaysOld(latestWeightDate);

            var latestBloodPressureDate = _healthService.GetLatestBloodPressureDate(DateTime.MinValue);
            var daysOldBloodpressure = DaysOld(latestBloodPressureDate);

            var latestRestingHeartRate = _healthService.GetLatestRestingHeartRateDate(DateTime.MinValue);
            var daysOldRestingHeartRate = DaysOld(latestRestingHeartRate);

            var latestDrinkDate = _healthService.GetLatestDrinkDate();
            var daysOldDrinkDate = DaysOld(latestDrinkDate);

            var latestExerciseDate = _healthService.GetLatestExerciseDate(DateTime.MinValue);
            var daysOldExerciseDate = DaysOld(latestExerciseDate);
            
            var messages = "";

            if (daysOldWeight > 4)
            {
                messages += $"Weight was last updated {daysOldWeight} days ago. ";
            }

            if (daysOldBloodpressure > 4)
            {
                messages += $"Blood pressure was last updated {daysOldBloodpressure} days ago. ";
            }

            if (daysOldRestingHeartRate > 2)
            {
                messages += $"Resting heart rate was last updated {daysOldRestingHeartRate} days ago. ";
            }

            if (daysOldDrinkDate > 3)
            {
                messages += $"Drinks were last updated {daysOldDrinkDate} days ago. ";
            }

            if (daysOldExerciseDate > 3)
            {
                messages += $"Exercise was last updated {daysOldExerciseDate} days ago. ";
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