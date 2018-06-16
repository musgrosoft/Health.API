using System.Threading.Tasks;

namespace Services.OAuth
{
    public interface IOAuthService
    {
        Task<string> GetFitbitRefreshToken();
        Task<string> GetGoogleRefreshToken();
        Task SaveFitbitAccessToken(string token);
        Task SaveFitbitRefreshToken(string token);
        Task SaveGoogleAccessToken(string token);
        Task SaveGoogleRefreshToken(string token);
    }
}