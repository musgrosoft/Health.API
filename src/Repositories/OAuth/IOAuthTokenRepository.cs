using System.Threading.Tasks;

namespace Repositories.OAuth
{
    public interface IOAuthTokenRepository
    {
        Task SaveToken(string tokenName, string tokenValue);
        Task<string> ReadToken(string tokenName);
    }
}
