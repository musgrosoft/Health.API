using System.Threading.Tasks;
using Repositories.OAuth.Models;

namespace Repositories.OAuth
{
    public class TokenRepository : ITokenRepository
    {
        private readonly HealthContext _healthContext;

        public TokenRepository(HealthContext healthContext)
        {
            _healthContext = healthContext;
        }

        public async Task UpsertToken(string tokenName, string tokenValue)
        {
            var existingToken = await _healthContext.Tokens.FindAsync(tokenName);

            if (existingToken != null)
            {
                existingToken.Value = tokenValue;
            }
            else
            {
                _healthContext.Add(new Token
                {
                    Name = tokenName,
                    Value = tokenValue
                });
            }

            _healthContext.SaveChanges();
        }

        public async Task<string> ReadToken(string tokenName)
        {
            var existingToken = await _healthContext.Tokens.FindAsync(tokenName);
            return existingToken.Value;
        }
    }
}