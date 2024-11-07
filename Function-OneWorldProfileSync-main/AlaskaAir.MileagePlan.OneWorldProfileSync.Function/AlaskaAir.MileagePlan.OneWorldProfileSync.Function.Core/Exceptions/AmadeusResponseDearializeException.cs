using System;
using System.Runtime.Serialization;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Exceptions
{
    [Serializable]
    class AmadeusResponseDearializeException: Exception
    {
        public AmadeusResponseDearializeException()
        {
        }

        public AmadeusResponseDearializeException(string message) : base(message)
        {
        }

        public AmadeusResponseDearializeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AmadeusResponseDearializeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
