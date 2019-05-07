using System;
using System.Threading.Tasks;
using Repositories.OAuth;
using Repositories.OAuth.Models;
using Xunit;

namespace Repository.Tests.Unit
{
    public class TokenRepositoryTests : IDisposable
    {
        private TokenRepository _tokenRepository;
        private FakeLocalContext _fakeLocalContext;

        public TokenRepositoryTests()
        {
            _fakeLocalContext = new FakeLocalContext();
            _tokenRepository = new TokenRepository(_fakeLocalContext);
        }

        public void Dispose()
        {
            _fakeLocalContext.Database.EnsureDeleted();
        }


        [Fact]
        public async Task ShouldGetTokenValue()
        {

            var token = new Token {Name = "TescoName", Value = "TescoValue"};

            _fakeLocalContext.Tokens.Add(token);

            _fakeLocalContext.SaveChanges();

            var result = await _tokenRepository.ReadToken("TescoName");

            Assert.Equal("TescoValue", result);

            
        }

        [Fact]
        public async Task ShouldInsertToken()
        {
            await _tokenRepository.UpsertToken("AsdaName", "AsdaValue");

            var token = _fakeLocalContext.Tokens.Find("AsdaName");

            Assert.Equal("AsdaValue", token.Value);


        }

        [Fact]
        public async Task ShouldUpdateToken()
        {
            await _tokenRepository.UpsertToken("AsdaName", "AsdaValue");

            var token = _fakeLocalContext.Tokens.Find("AsdaName");

            Assert.Equal("AsdaValue", token.Value);

            await _tokenRepository.UpsertToken("AsdaName", "AsdaValue2");

            var token2 = _fakeLocalContext.Tokens.Find("AsdaName");

            Assert.Equal("AsdaValue2", token2.Value);

        }

    }
}