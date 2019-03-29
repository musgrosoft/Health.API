using System.Threading.Tasks;

namespace Services.OAuth
{
    public interface ITokenService
    {
        Task<string> GetFitbitRefreshToken();
        Task SaveFitbitAccessToken(string token);
        Task SaveFitbitRefreshToken(string token);

        Task<string> GetWithingsRefreshToken();
        Task SaveWithingsAccessToken(string token);
        Task SaveWithingsRefreshToken(string token);
    }
}