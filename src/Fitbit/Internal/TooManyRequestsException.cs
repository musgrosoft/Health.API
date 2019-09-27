using System;

namespace Fitbit.Internal
{
    [Serializable]
    internal class TooManyRequestsException : Exception
    {
        internal TooManyRequestsException(string message) : base(message)
        {
        }
    }
}