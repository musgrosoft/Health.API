namespace Utils
{
    public interface ILogger
    {
        void Log(string message);
        void Error(Exception ex);
    }
}