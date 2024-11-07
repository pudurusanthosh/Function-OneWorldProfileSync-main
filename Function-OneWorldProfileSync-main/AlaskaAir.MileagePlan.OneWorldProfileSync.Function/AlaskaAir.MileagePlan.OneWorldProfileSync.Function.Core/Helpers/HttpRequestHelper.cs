using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers
{
    public class HttpRequestHelper : IHttpRequestHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _amadeusBaseUri;
        private readonly ILogger _logger;

        public HttpRequestHelper(IHttpClientFactory httpClientFactory, IConfigurationRoot config, ILogger<HttpRequestHelper> logger)
        {
            _httpClientFactory = httpClientFactory;
            _amadeusBaseUri = config["AmadeusBaseUri"];
            _logger = logger;
        }

        public async Task<T> PostHttpRequest<T>(string requestPayLoad, string soapAction)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = _httpClientFactory.CreateClient();

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _amadeusBaseUri);

            var headers = httpRequestMessage.Headers;

            headers.Accept.Clear();

            headers.Add("SOAPAction", soapAction);

            httpRequestMessage.Content = new StringContent(requestPayLoad, Encoding.UTF8, "text/xml");

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            try
            {
                httpResponseMessage.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException ex)
            {
                _logger.LogError($"Error calling Amadeus API request {JsonConvert.SerializeObject(requestPayLoad)} soapAction {soapAction}");
                throw new AmadeusRequestException(httpResponseMessage.StatusCode, ex.Message, ex.InnerException);
            }

            return await DeserializeAsync<T>(httpResponseMessage);
        }

        public async Task<T> DeserializeAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            var mySerializer = new XmlSerializer(typeof(T));
            var httpResponseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            T response;
            try
            {
                response = (T)mySerializer.Deserialize(httpResponseStream);
            }
            catch (Exception ex)
            {
                var httpResponseString = await httpResponseMessage.Content.ReadAsStringAsync();
                _logger.LogError($"Failed to desrialize amadeus response {httpResponseString}");
                throw new AmadeusResponseDearializeException(ex.Message, ex.InnerException);
            }
            return response;
        }
    }
}
