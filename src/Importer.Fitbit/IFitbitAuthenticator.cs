using System.Threading.Tasks;

namespace Importer.Fitbit
{
    public interface IFitbitAuthenticator
    {
        Task<string> GetAccessToken();
       // Task<FitbitRefreshTokenResponse> GetTokens(string refreshToken);
        Task SetTokens(string accessToken);
    }
}