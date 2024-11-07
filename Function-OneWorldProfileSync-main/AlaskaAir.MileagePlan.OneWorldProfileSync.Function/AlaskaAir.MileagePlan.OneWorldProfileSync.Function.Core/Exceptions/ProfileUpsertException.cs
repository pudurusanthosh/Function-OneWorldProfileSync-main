using System;
using System.Runtime.Serialization;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Exceptions
{
    [Serializable]
    public class ProfileUpsertException : Exception
    {
        public ProfileUpsertException()
        {
        }

        public ProfileUpsertException(string message) : base(message)
        {
        }

        public ProfileUpsertException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProfileUpsertException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
