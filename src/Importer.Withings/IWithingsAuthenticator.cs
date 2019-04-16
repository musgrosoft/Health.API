using System.Threading.Tasks;

namespace Importer.Withings
{
    public interface IWithingsAuthenticator
    {
        Task SetTokens(string authorizationCode);
        Task<string> GetAccessToken();
    }
}