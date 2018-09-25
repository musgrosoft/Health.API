using System.Threading.Tasks;

namespace Nokia.Services
{
    public interface INokiaAuthenticator
    {
        Task SetTokens(string authorizationCode);
        Task<string> GetAccessToken();
    }
}