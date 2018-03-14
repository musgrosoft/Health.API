using HealthAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HealthAPI.Controllers
{
    [Route("api/[controller]")]
    public class RestingHeartRatesController : Controller
    {
        private readonly HealthContext _context;

        public RestingHeartRatesController(HealthContext context)
        {
            _context = context;
        }

        // GET api/RestingHeartRates
        [HttpGet]
        public IEnumerable<ViewModels.RestingHeartRate> Get()
        {
            var beats = _context.RestingHeartRate.Select(x=>new ViewModels.RestingHeartRate
            {
                DateTime = x.DateTime,
                Beats = x.Beats
                    
            }).OrderBy(y=>y.DateTime).ToList();

            AddMovingAverages(beats, 10);

            return beats;

        }

        private void AddMovingAverages(List<ViewModels.RestingHeartRate> heartRates, int period)
        {
            for (int i = 0; i < heartRates.Count(); i++)
            {
                if (i >= period - 1)
                {
                    decimal total = 0;
                    for (int x = i; x > (i - period); x--)
                        total += heartRates[x].Beats;
                    decimal average = total / period;
                    // result.Add(series.Keys[i], average);
                    heartRates[i].MovingAverageBeats = average;
                }
                else
                {
                    //heartRates[i].MovingAverageBeats = heartRates[i].Beats;
                    heartRates[i].MovingAverageBeats = null;
                }

            }
        }


    }
}
