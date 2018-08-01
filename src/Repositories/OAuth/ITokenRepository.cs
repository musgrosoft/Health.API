using System.Threading.Tasks;

namespace Repositories.OAuth
{
    public interface ITokenRepository
    {
        Task SaveToken(string tokenName, string tokenValue);
        Task<string> ReadToken(string tokenName);
    }
}
