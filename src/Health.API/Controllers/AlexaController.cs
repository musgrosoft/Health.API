using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    public class AlexaController : Controller
    {
        // GET
        public IActionResult FlashBriefing()
        {
            var flashBriefings = new List<FlashBriiefing>
                {
                new FlashBriiefing
                {
                    uid = "EXAMPLE_CHANNEL_MULTI_ITEM_JSON_TTS_1",
                    updateDate = new DateTime(2016,4,10),
                    titleText = "Multi Item JSON (TTS)",
                    mainText = "This channel has multiple TTS JSON items. This is the first item.",
                    redirectionUrl = "https://www.amazon.com"

                },
                new FlashBriiefing
                {
                    uid = "EXAMPLE_CHANNEL_MULTI_ITEM_JSON_TTS_2",
                    updateDate = new DateTime(2016,4,10),
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