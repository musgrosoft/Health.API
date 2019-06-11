using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Services.Health;

namespace HealthAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Alexa")]
    public class AlexaController : Controller
    {
        private readonly HealthService _healthService;

        public AlexaController(HealthService healthService)
        {
            _healthService = healthService;
        }

        // GET
        [HttpGet]
        [Route("FlashBriefing")]
        public IActionResult FlashBriefing()
        {
            var latestWeight = _healthService.GetLatestWeight();
            var latestBloodPressure = _healthService.GetLatestBloodPressure();

            var flashBriefings = new List<FlashBriiefing>
                {
                new FlashBriiefing
                {
                    uid = "1_WEIGHT",
                    updateDate = latestWeight.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss0Z"), //"2019-06-10T22:34:51.0Z" ,
                    titleText = "Latest Weight",
                    mainText = $"Your weight from {latestWeight.CreatedDate.Day} {latestWeight.CreatedDate.Month} is {latestWeight.Kg} kg. " +
                               $"Your blood pressure from {latestBloodPressure.CreatedDate.Day} {latestBloodPressure.CreatedDate.Month} is {latestBloodPressure.Systolic} over {latestBloodPressure.Diastolic}. ",
                    redirectionUrl = "https://www.amazon.com"

                },
                new FlashBriiefing
                {
                    uid = "2_BLOOD_PRESSURE",
                    updateDate = latestBloodPressure.CreatedDate.ToString("yyyy-MM-ddTHH:mm:ss0Z"), //"2019-06-10T22:34:51.0Z" ,
                    titleText = "Latest Blood Pressure",
                    mainText = $"Your blood pressure from {latestBloodPressure.CreatedDate.Day} {latestBloodPressure.CreatedDate.Month} is {latestBloodPressure.Systolic} over {latestBloodPressure.Diastolic}",
                    redirectionUrl = "https://www.amazon.com"

                }

                };


            //Content-Type: application/json
            return Ok(flashBriefings);
        }
    }
}