﻿using Repositories.Health;
using Repositories.Models;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.MyHealth
{
    public class TargetService : ITargetService
    {
        private IHealthRepository _healthRepository;

        public TargetService(IHealthRepository healthRepository) {
            _healthRepository = healthRepository;
        }

        public List<TargetWeight> GetTargetWeights()
        {

            var targetStartDate = new DateTime(2018, 5, 1);
            var targetEndDate = DateTime.Now.AddDays(600);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;


            var allWeights = _healthRepository.GetLatestWeights((int)((DateTime.Now - targetStartDate).TotalDays) , DateTime.Now);
            
            var groups = allWeights.GroupBy(x => x.DateTime.Date);

            allWeights = groups.Select(x => new Weight
            {
                DateTime = x.Key.Date,
                Kg = x.Average(w => w.Kg),
                MovingAverageKg = x.Average(w => w.MovingAverageKg)
            }).ToList();

            var targetWeights = allWeights.Select(x => new TargetWeight
            {
                DateTime = x.DateTime,
                TargetKg = GetTargetWeight(x.DateTime),
                ActualKg = x.Kg,
                ActualMovingAverageKg = x.MovingAverageKg
            }).ToList();

            var futuredays = (targetEndDate - DateTime.Now).TotalDays;

            for (int i = 0; i < futuredays; i++)
            {
                var target = new TargetWeight
                {
                    DateTime = DateTime.Now.AddDays(i),
                    TargetKg = GetTargetWeight(DateTime.Now.AddDays(i))
                };

                targetWeights.Add(target);
            }

            return targetWeights.OrderBy(x=>x.DateTime).ToList();
        }


    public decimal? GetTargetWeight(DateTime dateTime)
        {
            var targets = new List<TargetWeight>();

            var targetStartDate = new DateTime(2018, 5, 1);
            var targetEndDate = DateTime.Now.AddDays(600);
            var totalDays = (targetEndDate - targetStartDate).TotalDays;

            var weightOnTargetStartDate = 90.74;
            var targetDailyWeightLoss = 0.5 / 30;
            var targetDailyWeightLoss2 = 0.25 / 30;

            var daysToHitHealthyWeight = 123;

            var daysDiff = (dateTime - targetStartDate).TotalDays;

            if (daysDiff < 0)
            {
                return null;
            }

            if (daysDiff <= daysToHitHealthyWeight)
            {
                return (decimal)(weightOnTargetStartDate - (daysDiff * targetDailyWeightLoss));
            }

            if (daysDiff <= totalDays)
            {
                return (decimal)((weightOnTargetStartDate - (daysToHitHealthyWeight * targetDailyWeightLoss + (daysDiff - daysToHitHealthyWeight) * targetDailyWeightLoss2)));
            }

            return null;


        }


    }
}
