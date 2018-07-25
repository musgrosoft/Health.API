using System.Threading.Tasks;
using Services.Fitbit.Domain;

namespace Services.Fitbit
{
    public interface IFitbitAuthenticator
    {
        Task<string> GetAccessToken();
       // Task<FitbitRefreshTokenResponse> GetTokens(string refreshToken);
    }
}