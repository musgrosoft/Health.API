using System;

namespace Fitbit
{
    [Serializable]
    public class TooManyRequestsException : Exception
    {
        public TooManyRequestsException(string message) : base(message)
        {
        }
    }
}