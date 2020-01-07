using System.Threading.Tasks;

namespace Fitbit
{
    public interface IFitbitAuthenticator
    {
        Task SetTokens(string authorizationCode);
        Task<string> GetAccessToken();
    }
}