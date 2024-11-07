using System;
using System.Runtime.Serialization;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Exceptions
{
    [Serializable]
    public class InvalidSessionException : Exception
    {
        public InvalidSessionException()
        {
        }

        public InvalidSessionException(string message) : base(message)
        {
        }

        public InvalidSessionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidSessionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
