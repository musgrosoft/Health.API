using System.Collections.Generic;
using System.Linq;
using HealthAPI.Models;
using HealthAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HealthAPI.Controllers
{
    [Route("api/[controller]")]
    public class BloodPressuresController : Controller
    {
        private readonly HealthContext _context;

        public BloodPressuresController(HealthContext context)
        {
            _context = context;
        }

        // GET api/bloodpressures
        [HttpGet]
        public IEnumerable<BloodPressure> Get()
        {
            var bloodPressures = _context.BloodPressures.OrderBy(x => x.DateTime).Select(x=> new BloodPressure { 
                
                DateTime = x.DateTime,
                Systolic = x.Systolic.Value,
                Diastolic = x.Diastolic.Value

                }).ToList();

            AddMovingAverages(bloodPressures, 10);

            return bloodPressures;
        }

        public void AddMovingAverages(List<BloodPressure> bloodPressures, int period)
        {
            for (int i = 0; i < bloodPressures.Count(); i++)
            {
                if (i >= period - 1)
                {
                    int systolicTotal = 0;
                    int diastolicTotal = 0;
                    for (int x = i; x > (i - period); x--)
                    {
                        systolicTotal += bloodPressures[x].Systolic;
                        diastolicTotal += bloodPressures[x].Diastolic;
                    }
                    int averageSystolic = systolicTotal / period;
                    int averageDiastolic = diastolicTotal / period;
                    // result.Add(series.Keys[i], average);
                    bloodPressures[i].MovingAverageSystolic = averageSystolic;
                    bloodPressures[i].MovingAverageDiastolic = averageDiastolic;
                }
                else
                {
                    //bloodPressures[i].MovingAverageSystolic = bloodPressures[i].Systolic;
                    //bloodPressures[i].MovingAverageDiastolic = bloodPressures[i].Diastolic;
                    bloodPressures[i].MovingAverageSystolic = null;
                    bloodPressures[i].MovingAverageDiastolic = null;
                }

            }
        }

    }
}
