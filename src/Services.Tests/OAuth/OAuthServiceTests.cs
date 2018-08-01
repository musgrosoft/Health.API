﻿using System.Threading.Tasks;
using Moq;
using Repositories.OAuth;
using Services.OAuth;
using Xunit;

namespace Services.Tests.OAuth
{
    public class OAuthServiceTests
    {
        private readonly Mock<ITokenRepository> _repo;
        private readonly OAuthService _oAuthService;

        public OAuthServiceTests()
        {
            _repo = new Mock<ITokenRepository>();
            _oAuthService = new OAuthService(_repo.Object);
        }

        [Fact]
        public async Task ShouldReadFitbitRefreshToken()
        {
            //Given
            _repo.Setup(x => x.ReadToken("fitbit_refresh_token")).Returns(Task.FromResult("Tremendous Token"));

            //When
            var token = await _oAuthService.GetFitbitRefreshToken();

            //Then
            Assert.Equal("Tremendous Token",token);
        }

        [Fact]
        public async Task ShouldSaveFitbitRefreshToken()
        {
            //Given
            _repo.Setup(x => x.SaveToken(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            //When
            await _oAuthService.SaveFitbitRefreshToken("Amazing Token");

            //Then
            _repo.Verify(x=>x.SaveToken("fitbit_refresh_token", "Amazing Token"),Times.Once);

        }

        [Fact]
        public async Task ShouldSaveFitbitAccessToken()
        {
            //Given
            _repo.Setup(x => x.SaveToken(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            //When
            await _oAuthService.SaveFitbitAccessToken("Good Token");

            //Then
            _repo.Verify(x => x.SaveToken("fitbit_access_token", "Good Token"), Times.Once);

        }

    }
}
