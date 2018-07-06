using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Repositories.Health;
using Repositories.Models;
using Services.Domain;
using Services.MyHealth;

namespace HealthAPI.Controllers.Migration
{
    public class TargetsController : Controller
    {
        private readonly IHealthRepository _healthRepository;
        private readonly ITargetService _targetService;

        public TargetsController(IHealthRepository healthRepository, ITargetService targetService)
        {
            _healthRepository = healthRepository;
            _targetService = targetService;
        }

        //[HttpGet]
        //public IActionResult ActivitySummaries()
        //{
        //    var targets = new List<ActivitySummary>();

        //    var targetStartDate = new DateTime(2017, 5, 2);
        //    var targetEndDate = DateTime.Now.AddDays(100);
        //    var totalDays = (targetEndDate - targetStartDate).TotalDays;

        //    var activeMinutesOnTargetStartDate = 0;
        //    var targetDailyActiveMinutes = 30;

        //    for (var i = 0; i <= totalDays; i++)
        //    {
        //        var target = new ActivitySummary
        //        {
        //            CreatedDate = targetStartDate.AddDays(i),
        //            CumSumActiveMinutes = (int)(activeMinutesOnTargetStartDate + (i * targetDailyActiveMinutes))
        //        };

        //        targets.Add(target);
        //    }

        //    return Json(targets);
        //}

        [HttpGet]
        public IActionResult AlcoholIntakes()
        {
            var targets = new List<TargetAlcoholIntake>();

            var targetStartDate = new DateTime(2018, 5, 29);
            var targetEndDate = DateTime.Now.AddDays(100);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var unitsOnTargetStartDate = 5148;
            var targetDailyUnits = 4;

            var allAlcoholIntakes = _healthRepository.GetAllAlcoholIntakes();

            for (var i = 0; i <= totalDays; i++)
            {
                var actualAlcoholIntake = allAlcoholIntakes.FirstOrDefault(x => x.CreatedDate.Date == targetStartDate.AddDays(i).Date);

                var target = new TargetAlcoholIntake
                {
                    DateTime = targetStartDate.AddDays(i),
                    TargetCumSumUnits = unitsOnTargetStartDate + (i * targetDailyUnits),
                    ActualCumSumUnits = actualAlcoholIntake?.CumSumUnits
                };

                targets.Add(target);
            }

            return Json(targets);
        }

        [HttpGet]
        public IActionResult HeartRateSummaries()
        {
            var targets = new List<HeartRateSummary>();

            var targetStartDate = new DateTime(2018, 5, 19);
            var targetEndDate = DateTime.Now.AddDays(100);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var minutesOnTargetStartDate = 1775;
            var targetDailyMinutes = 11;

            for (var i = 0; i <= totalDays; i++)
            {
                var target = new HeartRateSummary
                {
                    CreatedDate = targetStartDate.AddDays(i),
                    CumSumCardioAndAbove = (int)(minutesOnTargetStartDate + (i * targetDailyMinutes))
                };

                targets.Add(target);
            }

            return Json(targets);
        }

        //[HttpGet]
        //public IActionResult Weights()
        //{
        //    var targetWeights = _targetService.GetTargetWeights();

        //    return Json(targetWeights);
        //}


    }
}