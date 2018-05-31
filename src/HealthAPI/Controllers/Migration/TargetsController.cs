using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Repositories.Models;

namespace HealthAPI.Controllers.Migration
{
    public class TargetsController : Controller
    {
        // GET
        //public IActionResult Index()
        //{
        //    return
        //    View();
        //}

        [HttpGet]
        public IActionResult Weights()
        {
            var targets = new List<Weight>();

            var targetStartDate = new DateTime(2018, 5, 1);
            var targetEndDate = DateTime.Now.AddDays(100);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var weightOnTargetStartDate = 90.74;
            var targetDailyWeightLoss = 0.5 / 30;

            for (var i = 0 ; i <= totalDays ; i++)
            {
                var target = new Weight
                {
                    DateTime = targetStartDate.AddDays(i),
                    Kg = (Decimal)(weightOnTargetStartDate - (i * targetDailyWeightLoss))
                };

                targets.Add(target);
            }

            return Json(targets);
        }
    }
}