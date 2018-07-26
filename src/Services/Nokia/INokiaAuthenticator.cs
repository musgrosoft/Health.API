using System.Threading.Tasks;

namespace Services.Nokia
{
    public interface INokiaAuthenticator
    {
        Task SetTokens(string authorizationCode);
        Task<string> GetAccessToken();
    }
}