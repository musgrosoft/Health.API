using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Repositories.Health;
using Repositories.Models;

namespace HealthAPI.Controllers.Migration
{
    public class TargetsController : Controller
    {
        private readonly IHealthRepository _healthRepository;

        public TargetsController(IHealthRepository healthRepository)
        {
            _healthRepository = healthRepository;
        }

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

            var unitsOnTargetStartDate = 5148;
            var targetDailyUnits = 4;

            for (var i = 0; i <= totalDays; i++)
            {
                var target = new AlcoholIntake
                {
                    DateTime = targetStartDate.AddDays(i),
                    CumSumUnits = (Decimal)(unitsOnTargetStartDate + (i * targetDailyUnits))
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
            var targets = new List<TargetWeight>();

            var targetStartDate = new DateTime(2018, 5, 1);
            var targetEndDate = DateTime.Now.AddDays(100);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var weightOnTargetStartDate = 90.74;
            var targetDailyWeightLoss = 0.5 / 30;

            var allWeights = _healthRepository.GetAllWeights();

            for (var i = 0 ; i <= totalDays ; i++)
            {
                var actualWeight = allWeights.FirstOrDefault(x => x.DateTime.Date == targetStartDate.AddDays(i).Date);

                var target = new TargetWeight
                {
                    DateTime = targetStartDate.AddDays(i),
                    TargetKg = (Decimal)(weightOnTargetStartDate - (i * targetDailyWeightLoss)),
                    ActualKg = actualWeight?.Kg
                };

                targets.Add(target);
            }

            return Json(targets);
        }


        [HttpGet]
        public IActionResult StepCounts()
        {
            var targets = new List<StepCount>();

            var targetStartDate = new DateTime(2017, 5, 3);
            var targetEndDate = DateTime.Now.AddDays(100);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var stepsOnTargetStartDate = 0;
            var targetDailySteps = 10000;

            for (var i = 0; i <= totalDays; i++)
            {
                var target = new StepCount()
                {
                    DateTime = targetStartDate.AddDays(i),
                    CumSumCount = stepsOnTargetStartDate + (i * targetDailySteps)
                };

                targets.Add(target);
            }

            return Json(targets);
        }
    }
}