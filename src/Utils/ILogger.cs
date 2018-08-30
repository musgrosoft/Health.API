using System;
using System.Threading.Tasks;

namespace Utils
{
    public interface ILogger
    {
        Task LogAsync(string message);
        Task ErrorAsync(Exception ex);
    }
}