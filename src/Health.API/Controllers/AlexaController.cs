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
            var latest15MinuteErgoDistance = _healthService.GetLatest15MinuteErgos(1).First().Metres;


            var averageWeight = latestWeights.Average(x => x.Kg);

            var averageSystolic = latestBloodpressures.Average(x => x.Systolic);
            var averageDiastolic = latestBloodpressures.Average(x => x.Diastolic);

            var cumSumUnits = _healthService.GetCumSumUnits();



            var targetUnits = 5147.7 + ((DateTime.Now.Date - new DateTime(2018, 5, 29)).Days * 4);
            var target15MinuteErgoMetres = 3386 + (DateTime.Now.Date - new DateTime(2019, 1, 1)).TotalDays;
            var targetWeight = (86.000 - ((DateTime.Now - new DateTime(2019, 1, 1)).Days * (3.000 / 365)));

            var targetSystolic = 120;
            var targetDiastolic = 80;

            var hitMessages = "";
            var missedMessages = "";


            if (cumSumUnits < targetUnits)
            {
                hitMessages += $"Hitting drinks target, you are {(targetUnits - cumSumUnits):N0} units below target. ";
            }
            else
            {
                missedMessages += $"Missed drinks target, you are {(cumSumUnits - targetUnits):N0} units above target. ";
            }

            if (averageWeight < targetWeight)
            {
                hitMessages += $"Hitting weight target , target weight is {targetWeight:N1} and actual weight is {averageWeight:N1}, you are {(targetWeight - averageWeight):N1} kilograms below target, with a weight of {averageWeight:N1}. ";
            }
            else
            {
                missedMessages += $"Missed weight target , target weight is {targetWeight:N1} and actual weight is {averageWeight:N1}, you are {(averageWeight - targetWeight):N1} kilograms above target with a weight of {averageWeight:N1}. ";
            }

            if (averageSystolic > targetSystolic || averageDiastolic < targetDiastolic)
            {
                //todo systolic aand or diastolic in message
                missedMessages += $"Blood pressure is too high. At {(targetDiastolic - averageDiastolic):N0} over {(targetSystolic - averageSystolic):N0}";
            }
            else
            {
                hitMessages += $"Blood pressure is healthy, at {(targetDiastolic - averageDiastolic):N0} over {(targetSystolic - averageSystolic):N0}.";
            }

            if (target15MinuteErgoMetres > latest15MinuteErgoDistance)
            {
                missedMessages += $"Behind Ergo target by {target15MinuteErgoMetres - latest15MinuteErgoDistance} metres. ";
            }
            else
            {
                hitMessages += $"Ahead of Ergo target by {latest15MinuteErgoDistance - target15MinuteErgoMetres} metres. ";
            }

            missedMessages += "Placeholder for cumsum cardio minutes. ";
            missedMessages += "Placeholder for rowing target. ";
            missedMessages += "Placeholder for running target. ";

            var briefings = new List<FlashBriefing>();

            if (!string.IsNullOrWhiteSpace(hitMessages))
            {
                briefings.Add(

                    new FlashBriefing
                    {
                        uid = "3 HIT TARGETS",
                        updateDate = DateTime.Now.AddMinutes(3).ToString("yyyy-MM-ddTHH:mm:ss.0Z"),
                        titleText = "Hit Targets",
                        mainText = $"You have hit {hitMessages.Count()} targets. {hitMessages}",
                        redirectionUrl = "https://www.amazon.com"
                    }

                  );
            }

            if (!string.IsNullOrWhiteSpace(hitMessages))
            {
                briefings.Add(

                    new FlashBriefing
                    {
                        uid = "4 MISSED TARGETS",
                        updateDate = DateTime.Now.AddMinutes(4).ToString("yyyy-MM-ddTHH:mm:ss.0Z"),
                        titleText = "Missed Targets",
                        mainText = $"You have missed {missedMessages.Count()} targets. {missedMessages}",
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

            if (daysOldWeight > 2)
            {
                messages += $"Weight was last updated {daysOldWeight} days ago. ";
            }

            if (daysOldBloodpressure > 2)
            {
                messages += $"Blood pressure was last updated {daysOldBloodpressure} days ago. ";
            }

            if (daysOldRestingHeartRate > 2)
            {
                messages += $"Resting heart rate was last updated {daysOldRestingHeartRate} days ago. ";
            }

            if (daysOldDrinkDate > 2)
            {
                messages += $"Drinks were last updated {daysOldDrinkDate} days ago. ";
            }

            if (daysOldExerciseDate > 2)
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
                    uid = "2 OLD DATA",
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
                    uid = "1 TECHNICAL ERRORS",
                    updateDate = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-ddTHH:mm:ss.0Z"), 
                    titleText = "Technical Errors",
                    mainText = $"There are technical errors. {messages}",
                    redirectionUrl = "https://www.amazon.com"
                };
            }


        }
    }
}