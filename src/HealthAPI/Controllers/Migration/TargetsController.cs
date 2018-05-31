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
        public IActionResult ActivitySummaries()
        {
            var targets = new List<ActivitySummary>();

            var targetStartDate = new DateTime(2017, 5, 2);
            var targetEndDate = DateTime.Now.AddDays(100);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var activeMinutesOnTargetStartDate = 0;
            var targetDailyActiveMinutes = 30;

            for (var i = 0; i <= totalDays; i++)
            {
                var target = new ActivitySummary
                {
                    DateTime = targetStartDate.AddDays(i),
                    CumSumActiveMinutes = (int)(activeMinutesOnTargetStartDate + (i * targetDailyActiveMinutes))
                };

                targets.Add(target);
            }

            return Json(targets);
        }

        [HttpGet]
        public IActionResult AlcoholIntakes()
        {
            var targets = new List<AlcoholIntake>();

            var targetStartDate = new DateTime(2018, 5, 29);
            var targetEndDate = DateTime.Now.AddDays(100);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var unitsOnTargetStartDate = 5145;
            var targetDailyUnits = 4;

            for (var i = 0; i <= totalDays; i++)
            {
                var target = new AlcoholIntake
                {
                    DateTime = targetStartDate.AddDays(i),
                    CumSumUnits = (int)(unitsOnTargetStartDate + (i * targetDailyUnits))
                };

                targets.Add(target);
            }

            return Json(targets);
        }

        [HttpGet]
        public IActionResult HeartSummaries()
        {
            var targets = new List<HeartSummary>();

            var targetStartDate = new DateTime(2018, 5, 19);
            var targetEndDate = DateTime.Now.AddDays(100);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var minutesOnTargetStartDate = 1775;
            var targetDailyMinutes = 11;

            for (var i = 0; i <= totalDays; i++)
            {
                var target = new HeartSummary
                {
                    DateTime = targetStartDate.AddDays(i),
                    CumSumCardioAndAbove = (int)(minutesOnTargetStartDate + (i * targetDailyMinutes))
                };

                targets.Add(target);
            }

            return Json(targets);
        }

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