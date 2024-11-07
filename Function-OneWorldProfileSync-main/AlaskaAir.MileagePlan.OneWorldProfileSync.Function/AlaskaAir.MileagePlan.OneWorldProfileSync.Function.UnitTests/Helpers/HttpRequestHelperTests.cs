using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Net.Http;
using System.Text;

namespace AlaskaAir.MileagePlan.OWProfileSync.Function.UnitTests.Helpers
{
    [TestFixture]
    public class HttpRequestHelperTests
    {
        [Test]
        public async System.Threading.Tasks.Task Deserialize_Returns_SerializedMessageAsync()
        {
            var requestPayLoad = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                                    <SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"" xmlns:awss=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"">
                                        <SOAP-ENV:Header>
                                            <awss:Session>
                                                <awss:SessionId>00RB445I0O</awss:SessionId>
                                                <awss:SequenceNumber>1</awss:SequenceNumber>
                                                <awss:SecurityToken>3O2K2B5X0529GGHU8IJMXB6XO</awss:SecurityToken>
                                            </awss:Session>
                                            <wsa:To>http://www.w3.org/2005/08/addressing/anonymous</wsa:To>
                                            <wsa:From>
                                                <wsa:Address>https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS</wsa:Address>
                                            </wsa:From>
                                            <wsa:Action>http://webservices.amadeus.com/VLSSLQ_06_1_1A</wsa:Action>
                                            <wsa:MessageID>urn:uuid:42234e95-9660-bd94-75a0-5cb487d6b7c7</wsa:MessageID>
                                            <wsa:RelatesTo RelationshipType=""http://www.w3.org/2005/08/addressing/reply"">uuid:f6cdd15b-54ce-46b4-a8c3-b9097f356d64</wsa:RelatesTo>
                                        </SOAP-ENV:Header>
                                        <SOAP-ENV:Body>
                                            <Security_AuthenticateReply xmlns=""http://xml.amadeus.com/VLSSLR_06_1_1A"">
                                                <processStatus>
                                                    <statusCode>P</statusCode>
                                                </processStatus>
                                            </Security_AuthenticateReply>
                                        </SOAP-ENV:Body>
                                    </SOAP-ENV:Envelope>";

            Mock<HttpRequestHelper> httpRequestHelperMock = new Mock<HttpRequestHelper>(new Mock<IHttpClientFactory>().Object, new Mock<IConfigurationRoot>().Object, new Mock<ILogger<HttpRequestHelper>>().Object);

            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();

            httpResponseMessage.Content = new StringContent(requestPayLoad, Encoding.UTF8, "text/xml");

            var deserializedResponse = await httpRequestHelperMock.Object.DeserializeAsync<AuthResponse>(httpResponseMessage);

            Assert.IsNotNull(deserializedResponse);
        }
    }
}
