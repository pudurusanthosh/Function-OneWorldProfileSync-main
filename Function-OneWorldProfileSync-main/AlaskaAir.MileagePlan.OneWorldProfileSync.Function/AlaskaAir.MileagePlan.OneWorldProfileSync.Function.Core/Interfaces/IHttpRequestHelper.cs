using System.Net.Http;
using System.Threading.Tasks;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers
{
    public interface IHttpRequestHelper
    {
        Task<T> PostHttpRequest<T>(string requestPayLoad, string soapAction);

        Task<T> DeserializeAsync<T>(HttpResponseMessage httpResponseMessage);
    }
}