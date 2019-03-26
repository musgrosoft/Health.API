using System.Threading.Tasks;

namespace Services.Withings.Services
{
    public interface INokiaAuthenticator
    {
        Task SetTokens(string authorizationCode);
        Task<string> GetAccessToken();
    }
}