using System.Threading.Tasks;

namespace Withings
{
    public interface IWithingsAuthenticator
    {
        Task SetTokens(string authorizationCode);
        Task<string> GetAccessToken();
    }
}