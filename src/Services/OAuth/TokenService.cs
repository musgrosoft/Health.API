using System.Threading.Tasks;
using Repositories.OAuth;

namespace Services.OAuth
{
    public class TokenService : ITokenService
    {
        private const string FITBIT_ACCESS_TOKEN_KEY = "fitbit_access_token";
        private const string FITBIT_REFRESH_TOKEN_KEY = "fitbit_refresh_token";

        private const string NOKIA_ACCESS_TOKEN_KEY = "nokia_access_token";
        private const string NOKIA_REFRESH_TOKEN_KEY = "nokia_refresh_token";
        
        private readonly ITokenRepository _tokenRepository;
        
        public TokenService(ITokenRepository tokenRepository )
        {
            _tokenRepository = tokenRepository;
        }

        public async Task SaveFitbitAccessToken(string token)
        {
            await _tokenRepository.UpsertToken(FITBIT_ACCESS_TOKEN_KEY, token);
        }

        public async Task SaveFitbitRefreshToken(string token)
        {
            await _tokenRepository.UpsertToken(FITBIT_REFRESH_TOKEN_KEY, token);
        }
        
        public async Task<string> GetFitbitRefreshToken()
        {
            return await _tokenRepository.ReadToken(FITBIT_REFRESH_TOKEN_KEY);
        }


        public async Task<string> GetNokiaRefreshToken()
        {
            return await _tokenRepository.ReadToken(NOKIA_REFRESH_TOKEN_KEY);
        }

        public async Task SaveNokiaAccessToken(string token)
        {
            await _tokenRepository.UpsertToken(NOKIA_ACCESS_TOKEN_KEY, token);
        }

        public async Task SaveNokiaRefreshToken(string token)
        {
            await _tokenRepository.UpsertToken(NOKIA_REFRESH_TOKEN_KEY, token);
        }

    }
}
