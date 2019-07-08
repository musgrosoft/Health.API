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

            var furthest15MinuteErgo = _healthService.GetFurthest15MinuteErgo(DateTime.Now.AddDays(-15));
            var furthest30MinuteTreadmill = _healthService.GetFurthest30MinuteTreadmill(DateTime.Now.AddDays(-15));

            var averageWeight = latestWeights.Average(x => x.Kg);
            var averageSystolic = latestBloodpressures.Average(x => x.Systolic);
            var averageDiastolic = latestBloodpressures.Average(x => x.Diastolic);

            var cumSumUnits = _healthService.GetCumSumUnits();
            var cumSumCardioMinutes = _healthService.GetCumSumCardioMinutes();

            var target = _healthService.GetTarget(DateTime.Now);
            //var targetCumSumUnits

            var targetUnits = 5147.7 + ((DateTime.Now.Date - new DateTime(2018, 5, 29)).TotalDays * 4);
            var targetCardio = 11 * (DateTime.Now.Date - new DateTime(2018, 5, 29)).TotalDays;

            var hitMessages = new List<string>();
            var missedMessages = new List<string>();


            if (cumSumUnits < targetUnits)
            {
                hitMessages.Add( $"HIT TARGET. Drinks target, you are {(targetUnits - cumSumUnits):N0} units below target.");
            }
            else
            {
                missedMessages.Add($"MISSED TARGET. Drinks target, you are {(cumSumUnits - targetUnits):N0} units above target.");
            }

            if (cumSumCardioMinutes < targetCardio)
            {
                hitMessages.Add($"HIT TARGET. Cardio target, you are {(targetCardio - cumSumCardioMinutes):N0} minutes below target.");
            }
            else
            {
                missedMessages.Add($"MISSED TARGET. Cardio target, you are {(cumSumCardioMinutes - targetCardio):N0} minutes above target.");
            }


            if (averageWeight < target.Kg)
            {
                hitMessages.Add( $"HIT TARGET. Your weight is  {(target.Kg - averageWeight):N1} kilograms below target at {averageWeight:N1}.");
            }
            else
            {
                missedMessages.Add( $"MISSED TARGET. Your weight is {(averageWeight - target.Kg):N1} kilograms above target at {averageWeight:N1}.");
            }

            if (averageSystolic > target.Systolic || averageDiastolic < target.Diastolic)
            {
                //todo systolic aand or diastolic in message
                missedMessages.Add( $"MISSED TARGET. Blood pressure is too high. At {averageDiastolic:N0} over {averageSystolic:N0}.");
            }
            else
            {
                hitMessages.Add( $"HIT TARGET. Blood pressure is healthy, at {averageDiastolic:N0} over {averageSystolic:N0}.");
            }

            if (target.MetresErgo15Minutes > furthest15MinuteErgo.Metres)
            {
                missedMessages.Add( $"MISSED TARGET. Behind Ergo target by {target.MetresErgo15Minutes - furthest15MinuteErgo.Metres} metres.");
            }
            else
            {
                hitMessages.Add($"HIT TARGET. Ahead of Ergo target by {furthest15MinuteErgo.Metres - target.MetresErgo15Minutes} metres.");
            }
            
            if (target.MetresTreadmill30Minutes > furthest30MinuteTreadmill.Metres)
            {
                missedMessages.Add($"MISSED TARGET. Behind treadmill target by {target.MetresTreadmill30Minutes - furthest30MinuteTreadmill.Metres} metres.");
            }
            else
            {
                hitMessages.Add($"HIT TARGET. Ahead of treadmill target by {furthest30MinuteTreadmill.Metres - target.MetresTreadmill30Minutes} metres.");
            }

            
            var briefings = new List<FlashBriefing>();

            if (hitMessages.Any())
            {
                briefings.Add(

                    new FlashBriefing
                    {
                        uid = "3 HIT TARGETS",
                        updateDate = DateTime.Now.AddMinutes(3).ToString("yyyy-MM-ddTHH:mm:ss.0Z"),
                        titleText = "Hit Targets",
                        mainText = $"You have hit {hitMessages.Count()} targets. {string.Join("\n ", hitMessages)}",
                        redirectionUrl = "https://www.amazon.com"
                    }

                  );
            }

            if (missedMessages.Any())
            {
                briefings.Add(

                    new FlashBriefing
                    {
                        uid = "4 MISSED TARGETS",
                        updateDate = DateTime.Now.AddMinutes(4).ToString("yyyy-MM-ddTHH:mm:ss.0Z"),
                        titleText = "Missed Targets",
                        mainText = $"You have missed {missedMessages.Count()} targets. {string.Join(" ", missedMessages)}",
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
                messages += $"OLD DATA : Weight was last updated {daysOldWeight} days ago. ";
            }

            if (daysOldBloodpressure > 2)
            {
                messages += $"OLD DATA : Blood pressure was last updated {daysOldBloodpressure} days ago. ";
            }

            if (daysOldRestingHeartRate > 2)
            {
                messages += $"OLD DATA : Resting heart rate was last updated {daysOldRestingHeartRate} days ago. ";
            }

            if (daysOldDrinkDate > 2)
            {
                messages += $"OLD DATA : Drinks were last updated {daysOldDrinkDate} days ago. ";
            }

            if (daysOldExerciseDate > 2)
            {
                messages += $"OLD DATA : Exercise was last updated {daysOldExerciseDate} days ago. ";
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
                    titleText = "Old Data",
                    mainText = $"{messages}",
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
                messages += "TECHNICAL ERROR : Fitbit Refresh Token is empty.";
            }

            if (string.IsNullOrWhiteSpace(withingsRefreshToken))
            {
                messages += "TECHNICAL ERROR : Withings Refresh Token is empty.";
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
                    mainText = $"{messages}",
                    redirectionUrl = "https://www.amazon.com"
                };
            }


        }
    }
}