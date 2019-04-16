using System;

namespace Services.Fitbit
{
    [Serializable]
    public class TooManyRequestsException : Exception
    {
        public TooManyRequestsException(string message) : base(message)
        {
        }
    }
}