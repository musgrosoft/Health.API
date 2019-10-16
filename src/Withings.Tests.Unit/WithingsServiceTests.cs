using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Repositories.Health.Models;
using Withings.Domain;
using Xunit;

namespace Withings.Tests.Unit
{
    public class WithingsServiceTests
    {
        private Mock<IWithingsAuthenticator> _withingsAuthenticator;
        private Mock<IWithingsMapper> _withingsMapper;
        private Mock<IWithingsClientQueryAdapter> _withingsClientQueryAdaptor;
        private WithingsService _withingsService;
        private Mock<IWithingsClient> _withingsClient;

        public WithingsServiceTests()
        {
            _withingsAuthenticator = new Mock<IWithingsAuthenticator>();
            _withingsMapper = new Mock<IWithingsMapper>();
            _withingsClientQueryAdaptor = new Mock<IWithingsClientQueryAdapter>();
            _withingsClient = new Mock<IWithingsClient>();
            _withingsService = new WithingsService(_withingsAuthenticator.Object, _withingsMapper.Object, _withingsClientQueryAdaptor.Object, _withingsClient.Object);
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
            _withingsClientQueryAdaptor.Setup(x => x.GetMeasureGroups(It.IsAny<DateTime>(), It.IsAny<string>())).Returns(Task.FromResult(someMeasureGroups));
            _withingsMapper.Setup(x => x.MapToWeights(someMeasureGroups)).Returns(someWeights);

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
            _withingsClientQueryAdaptor.Setup(x => x.GetMeasureGroups(It.IsAny<DateTime>(), It.IsAny<string>())).Returns(Task.FromResult(someMeasureGroups));
            _withingsMapper.Setup(x => x.MapToBloodPressures(someMeasureGroups)).Returns(someBloodpressures);

            //When
            var bloodPressures = await _withingsService.GetBloodPressures(new DateTime());

            //Then
            Assert.Equal(someBloodpressures, bloodPressures);

        }

    }
}