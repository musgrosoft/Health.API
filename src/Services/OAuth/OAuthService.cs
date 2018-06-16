using System.Threading.Tasks;
using Repositories.OAuth;

namespace Services.OAuth
{
    public class OAuthService : IOAuthService
    {
        private const string FITBIT_ACCESS_TOKEN_KEY = "fitbit_access_token";
        private const string FITBIT_REFRESH_TOKEN_KEY = "fitbit_refresh_token";

        private const string GOOGLE_ACCESS_TOKEN_KEY = "google_access_token";
        private const string GOOGLE_REFRESH_TOKEN_KEY = "google_refresh_token";
        
        private readonly IOAuthTokenRepository _oAuthTokenRepository;
        
        public OAuthService(IOAuthTokenRepository oAuthTokenRepository )
        {
            _oAuthTokenRepository = oAuthTokenRepository;
        }

        public async Task SaveFitbitAccessToken(string token)
        {
            await _oAuthTokenRepository.SaveToken(FITBIT_ACCESS_TOKEN_KEY, token);
        }

        public async Task SaveFitbitRefreshToken(string token)
        {
            await _oAuthTokenRepository.SaveToken(FITBIT_REFRESH_TOKEN_KEY, token);
        }

        public async Task<string> GetFitbitRefreshToken()
        {
            return await _oAuthTokenRepository.ReadToken(FITBIT_REFRESH_TOKEN_KEY);
        }

        public async Task<string> GetGoogleRefreshToken()
        {
            return await _oAuthTokenRepository.ReadToken(GOOGLE_REFRESH_TOKEN_KEY);
        }

        public async Task SaveGoogleAccessToken(string token)
        {
            await _oAuthTokenRepository.SaveToken(GOOGLE_ACCESS_TOKEN_KEY, token);
        }

        public async Task SaveGoogleRefreshToken(string token)
        {
            await _oAuthTokenRepository.SaveToken(GOOGLE_REFRESH_TOKEN_KEY, token);
        }

        //public string GetFitbitAccessToken()
        //{
        //    return _oAuthTokenRepository.ReadToken(FITBIT_ACCESS_TOKEN_KEY);
        //}
    }
}
