using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Importer.Withings.Domain;
using Moq;
using Repositories.Health.Models;
using Xunit;

namespace Importer.Withings.Tests
{
    public class WithingsServiceTests
    {
        private Mock<IWithingsClient> _withingsClient;
        private Mock<IWithingsAuthenticator> _withingsAuthenticator;
        private Mock<IWithingsMapper> _withingsMapper;
        private Mock<IWithingsClientQueryAdapter> _withingsClientQueryAdaptor;
        private WithingsService _withingsService;

        public WithingsServiceTests()
        {
            _withingsClient = new Mock<IWithingsClient>();
            _withingsAuthenticator = new Mock<IWithingsAuthenticator>();
            _withingsMapper = new Mock<IWithingsMapper>();
            _withingsClientQueryAdaptor = new Mock<IWithingsClientQueryAdapter>();

            _withingsService = new WithingsService(_withingsClient.Object, _withingsAuthenticator.Object, _withingsMapper.Object, _withingsClientQueryAdaptor.Object);
        }

        [Fact]
        public async Task ShouldSubscribe()
        {
            //When
            await _withingsService.Subscribe();

            //Then
            _withingsClient.Verify(x => x.Subscribe(), Times.Once);
        }

        [Fact]
        public async Task ShouldSetTokens()
        {
            //When
            await _withingsService.SetTokens("elephant");

            //Then
            _withingsAuthenticator.Verify(x => x.SetTokens("elephant"), Times.Once);
        }

        [Fact]
        public async Task ShouldGetWeights()
        {
            //Given
            IEnumerable<Response.Measuregrp> someMeasureGroups = new List<Response.Measuregrp> { };
            var someWeights = new List<Weight>();
            _withingsClientQueryAdaptor.Setup(x => x.GetMeasureGroups(It.IsAny<DateTime>())).Returns(Task.FromResult(someMeasureGroups));
            _withingsMapper.Setup(x => x.MapMeasuresGroupsToWeights(someMeasureGroups)).Returns(someWeights);

            //When
            var weights = await _withingsService.GetWeights(new DateTime());

            //Then
            Assert.Equal(someWeights,weights);

        }

        [Fact]
        public async Task ShouldGetBloodPressures()
        {
            //Given
            IEnumerable<Response.Measuregrp> someMeasureGroups = new List<Response.Measuregrp> { };
            var someBloodpressures = new List<BloodPressure>();
            _withingsClientQueryAdaptor.Setup(x => x.GetMeasureGroups(It.IsAny<DateTime>())).Returns(Task.FromResult(someMeasureGroups));
            _withingsMapper.Setup(x => x.MapMeasuresGroupsToBloodPressures(someMeasureGroups)).Returns(someBloodpressures);

            //When
            var bloodPressures = await _withingsService.GetBloodPressures(new DateTime());

            //Then
            Assert.Equal(someBloodpressures, bloodPressures);

        }

    }
}