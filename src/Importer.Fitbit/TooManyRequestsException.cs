using System;

namespace Importer.Fitbit
{
    [Serializable]
    internal class TooManyRequestsException : Exception
    {
        internal TooManyRequestsException(string message) : base(message)
        {
        }
    }
}