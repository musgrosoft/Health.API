using System;

namespace Importer.Fitbit
{
    [Serializable]
    public class TooManyRequestsException : Exception
    {
        public TooManyRequestsException(string message) : base(message)
        {
        }
    }
}