using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;
using Services.MyHealth;

namespace HealthAPI.Controllers.Data
{
    [Produces("application/json")]
    [Route("api/Weights")]
    public class WeightsController : Controller
    {
        private readonly IHealthService _healthService;

        public WeightsController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<Weight>), 200)]
        public IActionResult Get()
        {
            return Json(_healthService.GetAllWeights());
        }

        //[HttpGet]
        //public IList<Weight> Get()
        //{
        //    return _healthService.GetAllWeights();
        //}

    }
}