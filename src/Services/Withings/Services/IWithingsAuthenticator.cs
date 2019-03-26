using System.Threading.Tasks;

namespace Services.Withings.Services
{
    public interface IWithingsAuthenticator
    {
        Task SetTokens(string authorizationCode);
        Task<string> GetAccessToken();
    }
}