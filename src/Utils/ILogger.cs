using System;
using System.Threading.Tasks;

namespace Utils
{
    public interface ILogger
    {
        Task LogMessageAsync(string message);
        Task LogErrorAsync(Exception ex);
    }
}