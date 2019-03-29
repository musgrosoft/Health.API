using System;

namespace Services.Fitbit.Services
{
    [Serializable]
    public class TooManyRequestsException : Exception
    {
        public TooManyRequestsException(string message) : base(message)
        {
        }
    }
}