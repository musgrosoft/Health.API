using System;
using System.Threading.Tasks;
using Repositories.Models;
using Repositories.OAuth;
using Xunit;

namespace Repository.Unit.Tests
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
        public async Task ShouldSaveToken()
        {
            await _tokenRepository.SaveToken("AsdaName", "AsdaValue");

            var token = _fakeLocalContext.Tokens.Find("AsdaName");

            Assert.Equal("AsdaValue", token.Value);


        }
    }
}