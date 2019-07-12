﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using HealthAPI.Controllers;
using Services.Health;
using Services.OAuth;

namespace Health.API.Controllers
{
    public interface IMessageMaker
    {
        List<string> GetOldDataMessages();
        Task<List<string>> GetTechnicalErrorMessages();
        TargetMessages GetTargetMessages();
    }

    public class MessageMaker : IMessageMaker
    {
        private readonly IHealthService _healthService;
        private readonly ITokenService _tokenService;

        public MessageMaker(IHealthService healthService, ITokenService tokenService)
        {
            _healthService = healthService;
            _tokenService = tokenService;
        }

        private double DaysOld(DateTime dateTime)
        {
            return (DateTime.Now.Date - dateTime.Date).TotalDays;
        }

        public List<string> GetOldDataMessages()
        {
            var latestWeightDate = _healthService.GetLatestWeightDate(DateTime.MinValue);
            var daysOldWeight = DaysOld(latestWeightDate);

            var latestBloodPressureDate = _healthService.GetLatestBloodPressureDate(DateTime.MinValue);
            var daysOldBloodpressure = DaysOld(latestBloodPressureDate);

            var latestRestingHeartRate = _healthService.GetLatestRestingHeartRateDate(DateTime.MinValue);
            var daysOldRestingHeartRate = DaysOld(latestRestingHeartRate);

            var latestDrinkDate = _healthService.GetLatestDrinkDate();
            var daysOldDrinkDate = DaysOld(latestDrinkDate);

            var latestExerciseDate = _healthService.GetLatestExerciseDate(DateTime.MinValue);
            var daysOldExerciseDate = DaysOld(latestExerciseDate);

            var messages = new List<string>();


            if (daysOldWeight > 2)
            {
                messages.Add($"OLD DATA : Weight was last updated {daysOldWeight} days ago. ");
            }

            if (daysOldBloodpressure > 2)
            {
                messages.Add($"OLD DATA : Blood pressure was last updated {daysOldBloodpressure} days ago. ");
            }

            if (daysOldRestingHeartRate > 2)
            {
                messages.Add($"OLD DATA : Resting heart rate was last updated {daysOldRestingHeartRate} days ago. ");
            }

            if (daysOldDrinkDate > 2)
            {
                messages.Add($"OLD DATA : Drinks were last updated {daysOldDrinkDate} days ago. ");
            }

            if (daysOldExerciseDate > 2)
            {
                messages.Add($"OLD DATA : Exercise was last updated {daysOldExerciseDate} days ago. ");
            }

            return messages;

        }

        public async Task<List<string>> GetTechnicalErrorMessages()
        {
            var fitbitRefreshToken = await _tokenService.GetFitbitRefreshToken();
            var withingsRefreshToken = await _tokenService.GetWithingsRefreshToken();

            var messages = new List<string>();

            if (string.IsNullOrWhiteSpace(fitbitRefreshToken))
            {
                messages.Add("TECHNICAL ERROR : Fitbit Refresh Token is empty.");
            }

            if (string.IsNullOrWhiteSpace(withingsRefreshToken))
            {
                messages.Add("TECHNICAL ERROR : Withings Refresh Token is empty.");
            }

            return messages;


        }



        public TargetMessages GetTargetMessages()
        {
            var daysForErgoAndRunningTargets = 15;

            var targetMessages = new TargetMessages();
            
            var latestWeights = _healthService.GetLatestWeights();
            var latestBloodpressures = _healthService.GetLatestBloodPressures();

            var furthest15MinuteErgo = _healthService.GetFurthest15MinuteErgo(DateTime.Now.AddDays(-daysForErgoAndRunningTargets));
            var furthest30MinuteTreadmill = _healthService.GetFurthest30MinuteTreadmill(DateTime.Now.AddDays(-daysForErgoAndRunningTargets));

            var averageWeight = latestWeights.Average(x => x.Kg);
            var averageSystolic = latestBloodpressures.Average(x => x.Systolic);
            var averageDiastolic = latestBloodpressures.Average(x => x.Diastolic);

            var cumSumUnits = _healthService.GetCumSumUnits();
            var cumSumCardioMinutes = _healthService.GetCumSumCardioMinutes();

            var target = _healthService.GetTarget(DateTime.Now);
            //var targetCumSumUnits

            var targetUnits = 5147.7 + ((DateTime.Now.Date - new DateTime(2018, 5, 29)).TotalDays * 4);
            var targetCardio = 11 * (DateTime.Now.Date - new DateTime(2018, 5, 29)).TotalDays;

            var target500mSplitInSec = 500 * 15 * 60 / target.MetresErgo15Minutes;
            var targetTimespan = new TimeSpan(0, 0, target500mSplitInSec);

            var current500mSplitInSec = 500 * 15 * 60 / furthest15MinuteErgo.Metres;
            var currentTimespan = new TimeSpan(0, 0, current500mSplitInSec);

            if (cumSumUnits < targetUnits)
            {
                targetMessages.HitTargets.Add($"HIT TARGET - DRINKS : Drinks target, you are {(targetUnits - cumSumUnits):N0} units below target.");
            }
            else
            {
                targetMessages.MissedTargets.Add($"MISSED TARGET - DRINKS : Drinks target, you are {(cumSumUnits - targetUnits):N0} units above target.");
            }

            if (cumSumCardioMinutes < targetCardio)
            {
                targetMessages.HitTargets.Add($"HIT TARGET - CARDIO MINUTES : Cardio target, you are {(targetCardio - cumSumCardioMinutes):N0} minutes below target.");
            }
            else
            {
                targetMessages.MissedTargets.Add($"MISSED TARGET - CARDIO MINUTES : Cardio target, you are {(cumSumCardioMinutes - targetCardio):N0} minutes above target. Target is {targetCardio} and actual is {cumSumCardioMinutes}. ");
            }


            if (averageWeight < target.Kg)
            {
                targetMessages.HitTargets.Add($"HIT TARGET - WEIGHT : Todays target weight is {(target.Kg):N1}. Your weight is {(target.Kg - averageWeight):N1} kilograms below target at {averageWeight:N1}.");
            }
            else
            {
                targetMessages.MissedTargets.Add($"MISSED TARGET - WEIGHT : Todays target weight is {(target.Kg):N1}. Your weight is {(averageWeight - target.Kg):N1} kilograms above target at {averageWeight:N1}.");
            }

            if (averageSystolic > target.Systolic || averageDiastolic < target.Diastolic)
            {
                //todo systolic aand or diastolic in message
                targetMessages.MissedTargets.Add($"MISSED TARGET - BLOOD PRESSURE. Blood pressure is too high at {averageDiastolic:N0}/{averageSystolic:N0} mmHg.");
            }
            else
            {
                targetMessages.HitTargets.Add($"HIT TARGET - BLOOD PRESSURE. Blood pressure is healthy, at {averageDiastolic:N0}/{averageSystolic:N0} mmHg.");
            }

            if (target.MetresErgo15Minutes > furthest15MinuteErgo.Metres)
            {
                targetMessages.MissedTargets.Add($"MISSED TARGET - ERGO : Today's 15 min ergo target is {target.MetresErgo15Minutes} (Equivalent to a 500m split time of {targetTimespan.Minutes}m{targetTimespan.Seconds}sec). You are behind Ergo target by {target.MetresErgo15Minutes - furthest15MinuteErgo.Metres} metres at {furthest15MinuteErgo.Metres} metres (Equivalent to a 500m split time of {currentTimespan.Minutes}m{currentTimespan.Seconds}sec) (furthest in last {daysForErgoAndRunningTargets} days).");
            }
            else
            {
                targetMessages.HitTargets.Add($"HIT TARGET - ERGO : Today's 15 min ergo target is {target.MetresErgo15Minutes}. You are ahead of Ergo target by {furthest15MinuteErgo.Metres - target.MetresErgo15Minutes} metres at {furthest15MinuteErgo.Metres} metres (Equivalent to a 500m split time of {currentTimespan.Minutes}m{currentTimespan.Seconds}sec) (furthest in last {daysForErgoAndRunningTargets} days).");
            }

            if (target.MetresTreadmill30Minutes > furthest30MinuteTreadmill.Metres)
            {
                targetMessages.MissedTargets.Add($"MISSED TARGET - TREADMILL. Today's 30 min treadmill target is {target.MetresTreadmill30Minutes}m ({(target.MetresTreadmill30Minutes* 0.000621371):N3}miles), your best distance for 30min in the last {daysForErgoAndRunningTargets} days is {furthest30MinuteTreadmill.Metres}m. You are behind treadmill target by {target.MetresTreadmill30Minutes - furthest30MinuteTreadmill.Metres} metres.");
            }
            else
            {
                targetMessages.HitTargets.Add($"HIT TARGET - TREADMILL.  Today's 30 min treadmill target is {target.MetresTreadmill30Minutes}m ({(target.MetresTreadmill30Minutes * 0.000621371):N3}miles), your best distance for 30min in the last {daysForErgoAndRunningTargets} days is {furthest30MinuteTreadmill.Metres}m. You are ahead of treadmill target by {furthest30MinuteTreadmill.Metres - target.MetresTreadmill30Minutes} metres.");
            }

            return targetMessages;

        }

    }

    public class TargetMessages
    {
        public TargetMessages()
        {
            HitTargets = new List<string>();
            MissedTargets = new List<string>();
        }

        public List<string> HitTargets { get; set; }
        public List<string> MissedTargets { get; set; }
    }
}