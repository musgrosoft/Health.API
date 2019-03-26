using System.Threading.Tasks;

namespace Services.Fitbit.Services
{
    public interface IFitbitAuthenticator
    {
        Task<string> GetAccessToken();
       // Task<FitbitRefreshTokenResponse> GetTokens(string refreshToken);
        Task SetTokens(string accessToken);
    }
}