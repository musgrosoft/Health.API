using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Health.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Alexa")]
    public class AlexaController : Controller
    {
        private readonly IAlexaMessageMaker _messageMaker;

        public AlexaController(IAlexaMessageMaker messageMaker)
        {
            _messageMaker = messageMaker;
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
            var targetMessages = _messageMaker.GetTargetMessages();

            var briefings = new List<FlashBriefing>();

            if (targetMessages.MissedTargets.Any())
            {
                briefings.Add(new FlashBriefing
                {
                    uid = "3 MISSED TARGETS",
                    updateDate = DateTime.Now.AddMinutes(3).ToString("yyyy-MM-ddTHH:mm:ss.0Z"),
                    titleText = "Missed Targets",
                    mainText = $"You have missed {targetMessages.MissedTargets.Count()} targets. {string.Join(" ", targetMessages.MissedTargets)}",
                    redirectionUrl = "https://www.amazon.com"
                });
            }

            if (targetMessages.HitTargets.Any())
            {

                briefings.Add(

                    new FlashBriefing
                    {
                        uid = "4 HIT TARGETS",
                        updateDate = DateTime.Now.AddMinutes(4).ToString("yyyy-MM-ddTHH:mm:ss.0Z"),
                        titleText = "Hit Targets",
                        mainText = $"You have hit {targetMessages.HitTargets.Count()} targets. {string.Join(" ", targetMessages.HitTargets)}",
                        redirectionUrl = "https://www.amazon.com"
                    }

                );
            }

            return briefings;

        }

        private FlashBriefing GetOldDataFlashBriefing()
        {
            var messages = _messageMaker.GetOldDataMessages();

            if (messages.Any())
            {
                return new FlashBriefing
                {
                    uid = "2 OLD DATA",
                    updateDate = DateTime.Now.AddMinutes(2).ToString("yyyy-MM-ddTHH:mm:ss.0Z"),
                    titleText = "Old Data",
                    mainText = string.Join(" ", messages),
                    redirectionUrl = "https://www.amazon.com"
                };
            }

            return null;

        }

        private async Task<FlashBriefing> GetTechnicalErrorFlashBriefing()
        {
            var messages = await _messageMaker.GetTechnicalErrorMessages();

            if (messages.Any())
            {
                return new FlashBriefing
                {
                    uid = "1 TECHNICAL ERRORS",
                    updateDate = DateTime.Now.AddMinutes(1).ToString("yyyy-MM-ddTHH:mm:ss.0Z"),
                    titleText = "Technical Errors",
                    mainText = string.Join(" ", messages),
                    redirectionUrl = "https://www.amazon.com"
                };
            }

            return null;

        }
    }
}