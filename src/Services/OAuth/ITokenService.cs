using System.Threading.Tasks;

namespace Services.OAuth
{
    public interface ITokenService
    {
        Task<string> GetFitbitRefreshToken();
        Task SaveFitbitAccessToken(string token);
        Task SaveFitbitRefreshToken(string token);

        Task<string> GetNokiaRefreshToken();
        Task SaveNokiaAccessToken(string token);
        Task SaveNokiaRefreshToken(string token);
    }
}