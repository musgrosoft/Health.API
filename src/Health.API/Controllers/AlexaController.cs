using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Alexa")]
    public class AlexaController : Controller
    {
        // GET
        [HttpGet]
        [Route("FlashBriefing")]
        public IActionResult FlashBriefing()
        {
            var flashBriefings = new List<FlashBriiefing>
                {
                new FlashBriiefing
                {
                    uid = "EXAMPLE_CHANNEL_MULTI_ITEM_JSON_TTS_1",
                    updateDate = "2019-06-10T22:34:51.0Z" ,
                    titleText = "Multi Item JSON (TTS)",
                    mainText = "This channel has multiple TTS JSON items. This is the first item.",
                    redirectionUrl = "https://www.amazon.com"

                },
                new FlashBriiefing
                {
                    uid = "EXAMPLE_CHANNEL_MULTI_ITEM_JSON_TTS_2",
                    updateDate = "2019-06-10T23:34:51.0Z",
                    titleText = "Multi Item JSON (TTS)",
                    mainText = "This channel has multiple TTS JSON items. This is the second item.",
                    redirectionUrl = "https://www.amazon.com"

                }

                };


            //Content-Type: application/json
            return Ok(flashBriefings);
        }
    }
}