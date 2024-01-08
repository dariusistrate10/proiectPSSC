using System;
using System.Runtime.Serialization;

namespace proiectPSSC.Domain.Exceptions
{
    [Serializable]
    internal class InvalidClientIDException : Exception
    {
        public InvalidClientIDException()
        {
        }

        public InvalidClientIDException(string? message) : base(message)
        {
        }

        public InvalidClientIDException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidClientIDException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

