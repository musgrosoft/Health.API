using System.Threading.Tasks;

namespace Fitbit.Services
{
    public interface IFitbitAuthenticator
    {
        Task<string> GetAccessToken();
       // Task<FitbitRefreshTokenResponse> GetTokens(string refreshToken);
        Task SetTokens(string accessToken);
    }
}