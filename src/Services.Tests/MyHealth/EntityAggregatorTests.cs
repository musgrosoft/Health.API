using System;
using System.Collections.Generic;
using System.Linq;
using Repositories.Models;
using Services.Health;
using Xunit;

namespace Services.Tests.MyHealth
{
    public class EntityAggregatorTests
    {
        [Fact]
        public void ShouldGroupStepCountsByWeek()
        {
            var entityAggregator = new EntityAggregator();

            var dailyStepCounts = new List<StepCount>
            {
                //week from monday
                new StepCount {CreatedDate = new DateTime(2018,7,16) , Count = 1000},
                new StepCount {CreatedDate = new DateTime(2018,7,17) , Count = 2000},
                new StepCount {CreatedDate = new DateTime(2018,7,18) , Count = 3000},
                new StepCount {CreatedDate = new DateTime(2018,7,19) , Count = 4000},
                new StepCount {CreatedDate = new DateTime(2018,7,20) , Count = 5000},
                new StepCount {CreatedDate = new DateTime(2018,7,21) , Count = 6000},
                new StepCount {CreatedDate = new DateTime(2018,7,22) , Count = 7000},

                //week from monday
                new StepCount {CreatedDate = new DateTime(2018,7,23) , Count = 8000},
                new StepCount {CreatedDate = new DateTime(2018,7,24) , Count = 9000},
                new StepCount {CreatedDate = new DateTime(2018,7,25) , Count = 10000},
                new StepCount {CreatedDate = new DateTime(2018,7,26) , Count = 11000},
                new StepCount {CreatedDate = new DateTime(2018,7,27) , Count = 12000},
                new StepCount {CreatedDate = new DateTime(2018,7,28) , Count = 13000},
                new StepCount {CreatedDate = new DateTime(2018,7,29) , Count = 14000},

                //partial week from monday
                new StepCount {CreatedDate = new DateTime(2018,7,30) , Count = 15000},
                new StepCount {CreatedDate = new DateTime(2018,7,31) , Count = 16000},
                new StepCount {CreatedDate = new DateTime(2018,8,1)  , Count = 17000},
            };

            var weeklyStepCounts = entityAggregator.GroupByWeek(dailyStepCounts);;

            Assert.Equal(3, weeklyStepCounts.Count);

            Assert.Equal(new DateTime(2018, 7, 16), weeklyStepCounts[0].CreatedDate);
            Assert.Equal(28000, weeklyStepCounts[0].Count);

            Assert.Equal(new DateTime(2018, 7, 23), weeklyStepCounts[1].CreatedDate);
            Assert.Equal(77000, weeklyStepCounts[1].Count);

            Assert.Equal(new DateTime(2018, 7, 30), weeklyStepCounts[2].CreatedDate);
            Assert.Equal(48000, weeklyStepCounts[2].Count);


        }

        [Fact]
        public void ShouldGroupStepCountsByMonth()
        {
            var entityAggregator = new EntityAggregator();

            var dailyStepCounts = new List<StepCount>
            {
                new StepCount {CreatedDate = new DateTime(2018,6,28)  , Count = 1},
                new StepCount {CreatedDate = new DateTime(2018,6,29)  , Count = 2},
                new StepCount {CreatedDate = new DateTime(2018,6,30)  , Count = 3},

                new StepCount {CreatedDate = new DateTime(2018,7,1) , Count = 4},
                new StepCount {CreatedDate = new DateTime(2018,7,2) , Count = 5},
                new StepCount {CreatedDate = new DateTime(2018,7,3) , Count = 6},
                new StepCount {CreatedDate = new DateTime(2018,7,4) , Count = 7},
                new StepCount {CreatedDate = new DateTime(2018,7,5) , Count = 8},
                new StepCount {CreatedDate = new DateTime(2018,7,6) , Count = 9},
                new StepCount {CreatedDate = new DateTime(2018,7,7) , Count = 10},
                new StepCount {CreatedDate = new DateTime(2018,7,8) , Count = 11},
                new StepCount {CreatedDate = new DateTime(2018,7,9) , Count = 12},
                new StepCount {CreatedDate = new DateTime(2018,7,10) , Count = 13},
                new StepCount {CreatedDate = new DateTime(2018,7,11) , Count = 14},
                new StepCount {CreatedDate = new DateTime(2018,7,12) , Count = 15},
                new StepCount {CreatedDate = new DateTime(2018,7,13) , Count = 16},
                new StepCount {CreatedDate = new DateTime(2018,7,14) , Count = 17},
                new StepCount {CreatedDate = new DateTime(2018,7,15) , Count = 18},
                new StepCount {CreatedDate = new DateTime(2018,7,16) , Count = 19},
                new StepCount {CreatedDate = new DateTime(2018,7,17) , Count = 20},
                new StepCount {CreatedDate = new DateTime(2018,7,18) , Count = 21},
                new StepCount {CreatedDate = new DateTime(2018,7,19) , Count = 22},
                new StepCount {CreatedDate = new DateTime(2018,7,20) , Count = 23},
                new StepCount {CreatedDate = new DateTime(2018,7,21) , Count = 24},
                new StepCount {CreatedDate = new DateTime(2018,7,22) , Count = 25},
                new StepCount {CreatedDate = new DateTime(2018,7,23) , Count = 26},
                new StepCount {CreatedDate = new DateTime(2018,7,24) , Count = 27},
                new StepCount {CreatedDate = new DateTime(2018,7,25) , Count = 28},
                new StepCount {CreatedDate = new DateTime(2018,7,26) , Count = 29},
                new StepCount {CreatedDate = new DateTime(2018,7,27) , Count = 30},
                new StepCount {CreatedDate = new DateTime(2018,7,28) , Count = 31},
                new StepCount {CreatedDate = new DateTime(2018,7,29) , Count = 32},
                new StepCount {CreatedDate = new DateTime(2018,7,30) , Count = 33},
                new StepCount {CreatedDate = new DateTime(2018,7,31) , Count = 34},

                new StepCount {CreatedDate = new DateTime(2018,8,1)  , Count = 35},
                new StepCount {CreatedDate = new DateTime(2018,8,2)  , Count = 36},
                new StepCount {CreatedDate = new DateTime(2018,8,3)  , Count = 37},
            };

            var weeklyStepCounts = entityAggregator.GroupByMonth(dailyStepCounts);;

            Assert.Equal(3, weeklyStepCounts.Count);

            Assert.Equal(new DateTime(2018, 6, 1), weeklyStepCounts[0].CreatedDate);
            Assert.Equal(2, weeklyStepCounts[0].Count);

            Assert.Equal(new DateTime(2018, 7, 1), weeklyStepCounts[1].CreatedDate);
            Assert.Equal(19, weeklyStepCounts[1].Count);

            Assert.Equal(new DateTime(2018, 8, 1), weeklyStepCounts[2].CreatedDate);
            Assert.Equal(36, weeklyStepCounts[2].Count);


        }

        [Fact]
        public void ShouldGroupHeartRateSummariesByWeek()
        {
            var entityAggregator = new EntityAggregator();

            var dailyStepCounts = new List<HeartRateSummary>
            {
                //week from monday
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,16) , CardioMinutes = 1, FatBurnMinutes = 11, PeakMinutes = 21},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,17) , CardioMinutes = 2, FatBurnMinutes = 12, PeakMinutes = 22},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,18) , CardioMinutes = 3, FatBurnMinutes = 13, PeakMinutes = 23},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,19) , CardioMinutes = 4, FatBurnMinutes = 14, PeakMinutes = 24},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,20) , CardioMinutes = 5, FatBurnMinutes = 15, PeakMinutes = 25},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,21) , CardioMinutes = 6, FatBurnMinutes = 16, PeakMinutes = 26},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,22) , CardioMinutes = 7, FatBurnMinutes = 17, PeakMinutes = 27},

                //week from monday
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,23) , CardioMinutes = 8, FatBurnMinutes = 18, PeakMinutes = 28},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,24) , CardioMinutes = 9, FatBurnMinutes = 19, PeakMinutes = 29},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,25) , CardioMinutes = 10, FatBurnMinutes = 20, PeakMinutes = 30},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,26) , CardioMinutes = 11, FatBurnMinutes = 21, PeakMinutes = 31},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,27) , CardioMinutes = 12, FatBurnMinutes = 22, PeakMinutes = 32},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,28) , CardioMinutes = 13, FatBurnMinutes = 23, PeakMinutes = 33},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,29) , CardioMinutes = 14, FatBurnMinutes = 24, PeakMinutes = 34},

                //partial week from monday
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,30) , CardioMinutes = 15, FatBurnMinutes = 25, PeakMinutes = 35},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,31) , CardioMinutes = 16, FatBurnMinutes = 26, PeakMinutes = 36},
                new HeartRateSummary {CreatedDate = new DateTime(2018,8,1)  , CardioMinutes = 17, FatBurnMinutes = 27, PeakMinutes = 37},
            };

            var weeklyStepCounts = entityAggregator.GroupByWeek(dailyStepCounts); ;

            Assert.Equal(3, weeklyStepCounts.Count);

            Assert.Equal(new DateTime(2018, 7, 16), weeklyStepCounts[0].CreatedDate);
            Assert.Equal(28, weeklyStepCounts[0].CardioMinutes);
            Assert.Equal(98, weeklyStepCounts[0].FatBurnMinutes);
            Assert.Equal(168, weeklyStepCounts[0].PeakMinutes);

            Assert.Equal(new DateTime(2018, 7, 23), weeklyStepCounts[1].CreatedDate);
            Assert.Equal(77, weeklyStepCounts[1].CardioMinutes);
            Assert.Equal(147, weeklyStepCounts[1].FatBurnMinutes);
            Assert.Equal(217, weeklyStepCounts[1].PeakMinutes);

            Assert.Equal(new DateTime(2018, 7, 30), weeklyStepCounts[2].CreatedDate);
            Assert.Equal(48, weeklyStepCounts[2].CardioMinutes);
            Assert.Equal(78, weeklyStepCounts[2].FatBurnMinutes);
            Assert.Equal(108, weeklyStepCounts[2].PeakMinutes);


        }

        [Fact]
        public void ShouldGroupHeartRateSummariesByMonth()
        {
            var entityAggregator = new EntityAggregator();

            var dailyStepCounts = new List<HeartRateSummary>
            {
                new HeartRateSummary {CreatedDate = new DateTime(2018,6,28)  , CardioMinutes = 1, FatBurnMinutes = 11, PeakMinutes = 21},
                new HeartRateSummary {CreatedDate = new DateTime(2018,6,29)  , CardioMinutes = 2, FatBurnMinutes = 12, PeakMinutes = 22},
                new HeartRateSummary {CreatedDate = new DateTime(2018,6,30)  , CardioMinutes = 3, FatBurnMinutes = 13, PeakMinutes = 23},

                new HeartRateSummary {CreatedDate = new DateTime(2018,7,1) , CardioMinutes = 4, FatBurnMinutes = 14, PeakMinutes = 24},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,2) , CardioMinutes = 5, FatBurnMinutes = 15, PeakMinutes = 25},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,3) , CardioMinutes = 6, FatBurnMinutes = 16, PeakMinutes = 26},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,4) , CardioMinutes = 7, FatBurnMinutes = 17, PeakMinutes = 27},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,5) , CardioMinutes = 8, FatBurnMinutes = 18, PeakMinutes = 28},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,6) , CardioMinutes = 9, FatBurnMinutes = 19, PeakMinutes = 29},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,7) , CardioMinutes = 10, FatBurnMinutes = 20, PeakMinutes = 30},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,8) , CardioMinutes = 11, FatBurnMinutes = 21, PeakMinutes = 31},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,9) , CardioMinutes = 12, FatBurnMinutes = 22, PeakMinutes = 32},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,10) , CardioMinutes = 13, FatBurnMinutes = 23, PeakMinutes = 33},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,11) , CardioMinutes = 14, FatBurnMinutes = 24, PeakMinutes = 34},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,12) , CardioMinutes = 15, FatBurnMinutes = 25, PeakMinutes = 35},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,13) , CardioMinutes = 16, FatBurnMinutes = 26, PeakMinutes = 36},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,14) , CardioMinutes = 17, FatBurnMinutes = 27, PeakMinutes = 37},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,15) , CardioMinutes = 18, FatBurnMinutes = 28, PeakMinutes = 38},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,16) , CardioMinutes = 19, FatBurnMinutes = 29, PeakMinutes = 39},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,17) , CardioMinutes = 20, FatBurnMinutes = 30, PeakMinutes = 40},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,18) , CardioMinutes = 21, FatBurnMinutes = 31, PeakMinutes = 41},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,19) , CardioMinutes = 22, FatBurnMinutes = 32, PeakMinutes = 42},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,20) , CardioMinutes = 23, FatBurnMinutes = 33, PeakMinutes = 43},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,21) , CardioMinutes = 24, FatBurnMinutes = 34, PeakMinutes = 44},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,22) , CardioMinutes = 25, FatBurnMinutes = 35, PeakMinutes = 45},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,23) , CardioMinutes = 26, FatBurnMinutes = 36, PeakMinutes = 46},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,24) , CardioMinutes = 27, FatBurnMinutes = 37, PeakMinutes = 47},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,25) , CardioMinutes = 28, FatBurnMinutes = 38, PeakMinutes = 48},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,26) , CardioMinutes = 29, FatBurnMinutes = 39, PeakMinutes = 49},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,27) , CardioMinutes = 30, FatBurnMinutes = 40, PeakMinutes = 50},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,28) , CardioMinutes = 31, FatBurnMinutes = 41, PeakMinutes = 51},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,29) , CardioMinutes = 32, FatBurnMinutes = 42, PeakMinutes = 52},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,30) , CardioMinutes = 33, FatBurnMinutes = 43, PeakMinutes = 53},
                new HeartRateSummary {CreatedDate = new DateTime(2018,7,31) , CardioMinutes = 34, FatBurnMinutes = 44, PeakMinutes = 54},

                new HeartRateSummary {CreatedDate = new DateTime(2018,8,1)  , CardioMinutes = 35, FatBurnMinutes = 45, PeakMinutes = 55},
                new HeartRateSummary {CreatedDate = new DateTime(2018,8,2)  , CardioMinutes = 36, FatBurnMinutes = 46, PeakMinutes = 56},
                new HeartRateSummary {CreatedDate = new DateTime(2018,8,3)  , CardioMinutes = 37, FatBurnMinutes = 47, PeakMinutes = 57},
            };

            var weeklyStepCounts = entityAggregator.GroupByMonth(dailyStepCounts); ;

            Assert.Equal(3, weeklyStepCounts.Count);

            Assert.Equal(new DateTime(2018, 6, 1), weeklyStepCounts[0].CreatedDate);
            Assert.Equal(2, weeklyStepCounts[0].CardioMinutes);
            Assert.Equal(12, weeklyStepCounts[0].FatBurnMinutes);
            Assert.Equal(22, weeklyStepCounts[0].PeakMinutes);

            Assert.Equal(new DateTime(2018, 7, 1), weeklyStepCounts[1].CreatedDate);
            Assert.Equal(19, weeklyStepCounts[1].CardioMinutes);
            Assert.Equal(29, weeklyStepCounts[1].FatBurnMinutes);
            Assert.Equal(39, weeklyStepCounts[1].PeakMinutes);

            Assert.Equal(new DateTime(2018, 8, 1), weeklyStepCounts[2].CreatedDate);
            Assert.Equal(36, weeklyStepCounts[2].CardioMinutes);
            Assert.Equal(46, weeklyStepCounts[2].FatBurnMinutes);
            Assert.Equal(56, weeklyStepCounts[2].PeakMinutes);


        }








        [Fact]
        public void ShouldGroupAlcoholIntakesByWeek()
        {
            var entityAggregator = new EntityAggregator();

            var dailyStepCounts = new List<AlcoholIntake>
            {
                //week from monday
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,16) , Units = 1000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,17) , Units = 2000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,18) , Units = 3000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,19) , Units = 4000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,20) , Units = 5000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,21) , Units = 6000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,22) , Units = 7000},

                //week from monday
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,23) , Units = 8000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,24) , Units = 9000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,25) , Units = 10000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,26) , Units = 11000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,27) , Units = 12000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,28) , Units = 13000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,29) , Units = 14000},

                //partial week from monday
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,30) , Units = 15000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,31) , Units = 16000},
                new AlcoholIntake {CreatedDate = new DateTime(2018,8,1)  , Units = 17000},
            };

            var weeklyStepCounts = entityAggregator.GroupByWeek(dailyStepCounts); ;

            Assert.Equal(3, weeklyStepCounts.Count);

            Assert.Equal(new DateTime(2018, 7, 16), weeklyStepCounts[0].CreatedDate);
            Assert.Equal(28000, weeklyStepCounts[0].Units);

            Assert.Equal(new DateTime(2018, 7, 23), weeklyStepCounts[1].CreatedDate);
            Assert.Equal(77000, weeklyStepCounts[1].Units);

            Assert.Equal(new DateTime(2018, 7, 30), weeklyStepCounts[2].CreatedDate);
            Assert.Equal(48000, weeklyStepCounts[2].Units);


        }

        [Fact]
        public void ShouldGroupAlcoholIntakesByMonth()
        {
            var entityAggregator = new EntityAggregator();

            var dailyStepCounts = new List<AlcoholIntake>
            {
                new AlcoholIntake {CreatedDate = new DateTime(2018,6,28)  , Units = 1},
                new AlcoholIntake {CreatedDate = new DateTime(2018,6,29)  , Units = 2},
                new AlcoholIntake {CreatedDate = new DateTime(2018,6,30)  , Units = 3},

                new AlcoholIntake {CreatedDate = new DateTime(2018,7,1) , Units = 4},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,2) , Units = 5},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,3) , Units = 6},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,4) , Units = 7},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,5) , Units = 8},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,6) , Units = 9},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,7) , Units = 10},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,8) , Units = 11},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,9) , Units = 12},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,10) , Units = 13},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,11) , Units = 14},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,12) , Units = 15},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,13) , Units = 16},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,14) , Units = 17},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,15) , Units = 18},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,16) , Units = 19},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,17) , Units = 20},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,18) , Units = 21},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,19) , Units = 22},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,20) , Units = 23},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,21) , Units = 24},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,22) , Units = 25},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,23) , Units = 26},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,24) , Units = 27},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,25) , Units = 28},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,26) , Units = 29},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,27) , Units = 30},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,28) , Units = 31},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,29) , Units = 32},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,30) , Units = 33},
                new AlcoholIntake {CreatedDate = new DateTime(2018,7,31) , Units = 34},

                new AlcoholIntake {CreatedDate = new DateTime(2018,8,1)  , Units = 35},
                new AlcoholIntake {CreatedDate = new DateTime(2018,8,2)  , Units = 36},
                new AlcoholIntake {CreatedDate = new DateTime(2018,8,3)  , Units = 37},
            };

            var weeklyStepCounts = entityAggregator.GroupByMonth(dailyStepCounts); ;

            Assert.Equal(3, weeklyStepCounts.Count);

            Assert.Equal(new DateTime(2018, 6, 1), weeklyStepCounts[0].CreatedDate);
            Assert.Equal(2, weeklyStepCounts[0].Units);

            Assert.Equal(new DateTime(2018, 7, 1), weeklyStepCounts[1].CreatedDate);
            Assert.Equal(19, weeklyStepCounts[1].Units);

            Assert.Equal(new DateTime(2018, 8, 1), weeklyStepCounts[2].CreatedDate);
            Assert.Equal(36, weeklyStepCounts[2].Units);


        }





        [Fact]
        public void ShouldGroupActivitySummariesByWeek()
        {
            var entityAggregator = new EntityAggregator();

            var dailyStepCounts = new List<ActivitySummary>
            {
                //week from monday
                new ActivitySummary {CreatedDate = new DateTime(2018,7,16) , FairlyActiveMinutes = 1, VeryActiveMinutes = 11},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,17) , FairlyActiveMinutes = 2, VeryActiveMinutes = 12},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,18) , FairlyActiveMinutes = 3, VeryActiveMinutes = 13},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,19) , FairlyActiveMinutes = 4, VeryActiveMinutes = 14},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,20) , FairlyActiveMinutes = 5, VeryActiveMinutes = 15},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,21) , FairlyActiveMinutes = 6, VeryActiveMinutes = 16},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,22) , FairlyActiveMinutes = 7, VeryActiveMinutes = 17},

                //week from monday
                new ActivitySummary {CreatedDate = new DateTime(2018,7,23) , FairlyActiveMinutes = 8, VeryActiveMinutes = 18},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,24) , FairlyActiveMinutes = 9, VeryActiveMinutes = 19},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,25) , FairlyActiveMinutes = 10, VeryActiveMinutes = 20},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,26) , FairlyActiveMinutes = 11, VeryActiveMinutes = 21},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,27) , FairlyActiveMinutes = 12, VeryActiveMinutes = 22},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,28) , FairlyActiveMinutes = 13, VeryActiveMinutes = 23},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,29) , FairlyActiveMinutes = 14, VeryActiveMinutes = 24},

                //partial week from monday
                new ActivitySummary {CreatedDate = new DateTime(2018,7,30) , FairlyActiveMinutes = 15, VeryActiveMinutes = 25},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,31) , FairlyActiveMinutes = 16, VeryActiveMinutes = 26},
                new ActivitySummary {CreatedDate = new DateTime(2018,8,1)  , FairlyActiveMinutes = 17, VeryActiveMinutes = 27},
            };

            var weeklyStepCounts = entityAggregator.GroupByWeek(dailyStepCounts); ;

            Assert.Equal(3, weeklyStepCounts.Count);

            Assert.Equal(new DateTime(2018, 7, 16), weeklyStepCounts[0].CreatedDate);
            Assert.Equal(28, weeklyStepCounts[0].FairlyActiveMinutes);
            Assert.Equal(98, weeklyStepCounts[0].VeryActiveMinutes);
            

            Assert.Equal(new DateTime(2018, 7, 23), weeklyStepCounts[1].CreatedDate);
            Assert.Equal(77, weeklyStepCounts[1].FairlyActiveMinutes);
            Assert.Equal(147, weeklyStepCounts[1].VeryActiveMinutes);
            

            Assert.Equal(new DateTime(2018, 7, 30), weeklyStepCounts[2].CreatedDate);
            Assert.Equal(48, weeklyStepCounts[2].FairlyActiveMinutes);
            Assert.Equal(78, weeklyStepCounts[2].VeryActiveMinutes);
            


        }

        [Fact]
        public void ShouldGroupActivitySummariesByMonth()
        {
            var entityAggregator = new EntityAggregator();

            var dailyStepCounts = new List<ActivitySummary>
            {
                new ActivitySummary {CreatedDate = new DateTime(2018,6,28)  , FairlyActiveMinutes = 1, VeryActiveMinutes = 11},
                new ActivitySummary {CreatedDate = new DateTime(2018,6,29)  , FairlyActiveMinutes = 2, VeryActiveMinutes = 12},
                new ActivitySummary {CreatedDate = new DateTime(2018,6,30)  , FairlyActiveMinutes = 3, VeryActiveMinutes = 13},

                new ActivitySummary {CreatedDate = new DateTime(2018,7,1) , FairlyActiveMinutes = 4, VeryActiveMinutes = 14},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,2) , FairlyActiveMinutes = 5, VeryActiveMinutes = 15},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,3) , FairlyActiveMinutes = 6, VeryActiveMinutes = 16},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,4) , FairlyActiveMinutes = 7, VeryActiveMinutes = 17},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,5) , FairlyActiveMinutes = 8, VeryActiveMinutes = 18},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,6) , FairlyActiveMinutes = 9, VeryActiveMinutes = 19},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,7) , FairlyActiveMinutes = 10, VeryActiveMinutes = 20},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,8) , FairlyActiveMinutes = 11, VeryActiveMinutes = 21},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,9) , FairlyActiveMinutes = 12, VeryActiveMinutes = 22},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,10) , FairlyActiveMinutes = 13, VeryActiveMinutes = 23},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,11) , FairlyActiveMinutes = 14, VeryActiveMinutes = 24},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,12) , FairlyActiveMinutes = 15, VeryActiveMinutes = 25},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,13) , FairlyActiveMinutes = 16, VeryActiveMinutes = 26},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,14) , FairlyActiveMinutes = 17, VeryActiveMinutes = 27},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,15) , FairlyActiveMinutes = 18, VeryActiveMinutes = 28},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,16) , FairlyActiveMinutes = 19, VeryActiveMinutes = 29},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,17) , FairlyActiveMinutes = 20, VeryActiveMinutes = 30},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,18) , FairlyActiveMinutes = 21, VeryActiveMinutes = 31},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,19) , FairlyActiveMinutes = 22, VeryActiveMinutes = 32},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,20) , FairlyActiveMinutes = 23, VeryActiveMinutes = 33},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,21) , FairlyActiveMinutes = 24, VeryActiveMinutes = 34},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,22) , FairlyActiveMinutes = 25, VeryActiveMinutes = 35},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,23) , FairlyActiveMinutes = 26, VeryActiveMinutes = 36},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,24) , FairlyActiveMinutes = 27, VeryActiveMinutes = 37},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,25) , FairlyActiveMinutes = 28, VeryActiveMinutes = 38},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,26) , FairlyActiveMinutes = 29, VeryActiveMinutes = 39},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,27) , FairlyActiveMinutes = 30, VeryActiveMinutes = 40},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,28) , FairlyActiveMinutes = 31, VeryActiveMinutes = 41},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,29) , FairlyActiveMinutes = 32, VeryActiveMinutes = 42},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,30) , FairlyActiveMinutes = 33, VeryActiveMinutes = 43},
                new ActivitySummary {CreatedDate = new DateTime(2018,7,31) , FairlyActiveMinutes = 34, VeryActiveMinutes = 44},

                new ActivitySummary {CreatedDate = new DateTime(2018,8,1)  , FairlyActiveMinutes = 35, VeryActiveMinutes = 45},
                new ActivitySummary {CreatedDate = new DateTime(2018,8,2)  , FairlyActiveMinutes = 36, VeryActiveMinutes = 46},
                new ActivitySummary {CreatedDate = new DateTime(2018,8,3)  , FairlyActiveMinutes = 37, VeryActiveMinutes = 47},
            };

            var weeklyStepCounts = entityAggregator.GroupByMonth(dailyStepCounts); ;

            Assert.Equal(3, weeklyStepCounts.Count);

            Assert.Equal(new DateTime(2018, 6, 1), weeklyStepCounts[0].CreatedDate);
            Assert.Equal(2, weeklyStepCounts[0].FairlyActiveMinutes);
            Assert.Equal(12, weeklyStepCounts[0].VeryActiveMinutes);
            

            Assert.Equal(new DateTime(2018, 7, 1), weeklyStepCounts[1].CreatedDate);
            Assert.Equal(19, weeklyStepCounts[1].FairlyActiveMinutes);
            Assert.Equal(29, weeklyStepCounts[1].VeryActiveMinutes);
            

            Assert.Equal(new DateTime(2018, 8, 1), weeklyStepCounts[2].CreatedDate);
            Assert.Equal(36, weeklyStepCounts[2].FairlyActiveMinutes);
            Assert.Equal(46, weeklyStepCounts[2].VeryActiveMinutes);
            


        }






    }
}