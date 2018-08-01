﻿using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.OAuth
{
    public class TokenRepository : IOAuthTokenRepository
    {
        private readonly HealthContext _healthContext;

        public TokenRepository(HealthContext healthContext)
        {
            _healthContext = healthContext;
        }

        public async Task SaveToken(string tokenName, string tokenValue)
        {
            var existingToken = _healthContext.Tokens.Find(tokenName);

            if (existingToken != null)
            {
                existingToken.Value = tokenValue;
            }
            else
            {
                //  _logger.Log("HEART SUMMARY : insert thing");
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