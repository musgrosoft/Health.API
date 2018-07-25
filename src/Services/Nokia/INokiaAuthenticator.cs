using System.Threading.Tasks;

namespace Services.Nokia
{
    public interface INokiaAuthenticator
    {
        Task<string> GetAccessToken();
    }
}