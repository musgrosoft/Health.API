using System.Threading.Tasks;

namespace Services.Withings
{
    public interface IWithingsAuthenticator
    {
        Task SetTokens(string authorizationCode);
        Task<string> GetAccessToken();
    }
}