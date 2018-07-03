using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repositories.Health;
using Repositories.Models;

namespace HealthAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Weights")]
    public class WeightsController : Controller
    {
        private readonly IHealthRepository _healthRepository;

        public WeightsController(IHealthRepository healthRepository)
        {
            _healthRepository = healthRepository;
        }

        [HttpGet]
        public IList<Weight> Get()
        {
            return _healthRepository.GetAllWeights();
        }


    }
}