using System;
using System.Net;
using System.Runtime.Serialization;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Exceptions
{
    [Serializable]
    public class AmadeusRequestException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; private set; }
        public AmadeusRequestException(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
        }

        public AmadeusRequestException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        public AmadeusRequestException(HttpStatusCode httpStatusCode, string message, Exception innerException) : base(message, innerException)
        {
            HttpStatusCode = httpStatusCode;
        }

        protected AmadeusRequestException(HttpStatusCode httpStatusCode, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
