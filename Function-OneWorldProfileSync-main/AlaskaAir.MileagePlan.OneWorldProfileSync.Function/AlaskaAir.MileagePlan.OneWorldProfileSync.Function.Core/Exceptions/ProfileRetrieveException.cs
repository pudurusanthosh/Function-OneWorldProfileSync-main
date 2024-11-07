using System;
using System.Runtime.Serialization;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Exceptions
{
    [Serializable]
    public class ProfileRetrieveException : Exception
    {
        public ProfileRetrieveException()
        {
        }

        public ProfileRetrieveException(string message) : base(message)
        {
        }

        public ProfileRetrieveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProfileRetrieveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
