using System;
using System.Runtime.Serialization;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Exceptions
{
    [Serializable]
    public class ProfileDeleteException : Exception
    {
        public ProfileDeleteException()
        {
        }

        public ProfileDeleteException(string message) : base(message)
        {
        }

        public ProfileDeleteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProfileDeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
