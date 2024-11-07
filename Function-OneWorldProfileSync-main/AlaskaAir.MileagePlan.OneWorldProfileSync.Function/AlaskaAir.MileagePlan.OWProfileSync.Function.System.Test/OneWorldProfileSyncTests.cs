using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Processors;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlaskaAir.MileagePlan.OWProfileSync.Function.System.Test
{
    [TestFixture]
    public class OneWorldProfileSyncTests
    {
        private QueueClient _QueueClient;
        private ProfilePublisherRequest _profilePublisherRequest;
        private readonly string _connectionString = Environment.GetEnvironmentVariable("ServiceBusConnectionString");
        private readonly string _memberProfileUpdateQueue = "as.sb.lcc.memberprofileupdate.queue"; // Amadeus Queue
        private AmadeusOperations _amadeusOperations;

        [SetUp]
        public void Init()
        {
            _QueueClient = new QueueClient(_connectionString, _memberProfileUpdateQueue);
            _profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "1",
                MileagePlanNumber = DateTime.Now.ToString("yyyyMMddHHmmss"),
                EventType = "TIER_UPGRADE",
                TierId = "3",
                TierName = "MVP",
                LastName = "EMERALD",
                FirstName = "TESTMEMBER",
                OneWorldTier = "EMERALD",
                MemberType = "Individual"
            };

            Mock<IHttpClientFactory> ihttpClientFactoryMock = new Mock<IHttpClientFactory>();

            ihttpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

            var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
            Environment.SetEnvironmentVariable("amadeusPassword", Environment.GetEnvironmentVariable("AmadeusPassword"));
            Environment.SetEnvironmentVariable("wsaTo", uri);
            Environment.SetEnvironmentVariable("amadeusBaseUri", uri);
            Environment.SetEnvironmentVariable("sourceOffice", "SEAAS18PA");
            Environment.SetEnvironmentVariable("wsaTo", "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS");
            Environment.SetEnvironmentVariable("originator", "WSASLOY");

            var config = new ConfigurationBuilder()
                            .AddEnvironmentVariables()
                            .Build();

            Mock<ILogger<HttpRequestHelper>> iLoggerMock = new Mock<ILogger<HttpRequestHelper>>();
            Mock<ILogger<AmadeusOperations>> iLoggerAmadeusOperationMock = new Mock<ILogger<AmadeusOperations>>();

            IHttpRequestHelper httpRequestHelper = new HttpRequestHelper(ihttpClientFactoryMock.Object, config, iLoggerMock.Object);

            _amadeusOperations = new AmadeusOperations(httpRequestHelper, new AmadeusRequestCreationHelper(config), iLoggerAmadeusOperationMock.Object);
        }

        [Test]
        public async Task AmadeusSyncRefreshTest()
        {
            // INSERT PROFILE

            // ARRANGE

            var input = JsonConvert.SerializeObject(_profilePublisherRequest);

            // ACT

            await _QueueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(input)));

            Thread.Sleep(1000);
            Session session =null;

            while (session == null)
            {
                session = await _amadeusOperations.Authenticate();
            }

            int retryCount = 10;

            AMA_ProfileReadRS actual = null;

            while (retryCount-- > 0 && actual == null)
            {
                Thread.Sleep(3000);
                actual = await _amadeusOperations.ProfileRetrieve(session, _profilePublisherRequest);
            }

            // ASSERT

            Assert.IsNotNull(actual);

            var loyaltyInfo = actual.Profiles.ProfileInfo.UniqueID.Where(x => x.ID_Context == "LOYALTYNUMBER");

            var personName = actual.Profiles.ProfileInfo.Profile.Customer.PersonName;
            var customerLoyalty = actual.Profiles.ProfileInfo.Profile.Customer.CustLoyalty;

            Assert.AreEqual(_profilePublisherRequest.MileagePlanNumber, loyaltyInfo.FirstOrDefault().ID);
            Assert.AreEqual(_profilePublisherRequest.FirstName, personName.GivenName);
            Assert.AreEqual(_profilePublisherRequest.LastName, personName.Surname);
            Assert.AreEqual("EMER", customerLoyalty.LoyalLevelDescription);

            // REFRESH PROFILE

            // ARRANGE
            _profilePublisherRequest.EventType = "BATCH_REFRESH";
            _profilePublisherRequest.TierId = "1";
            _profilePublisherRequest.OneWorldTier = "RUBY";
            _profilePublisherRequest.FirstName = "UPDFIRSTNAME";
            _profilePublisherRequest.LastName = "UPDLASTNAME";
            _profilePublisherRequest.ModificationNumber = "2";

            input = JsonConvert.SerializeObject(_profilePublisherRequest);

            session = await _amadeusOperations.Authenticate();

            // ACT

            await _QueueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(input)));

            retryCount = 10;

            actual = null;

            while (retryCount-- > 0 && actual?.Profiles?.ProfileInfo?.Profile?.Customer?.PersonName?.GivenName != _profilePublisherRequest.FirstName)
            {
                Thread.Sleep(3000);
                actual = await _amadeusOperations.ProfileRetrieve(session, _profilePublisherRequest);
            }

            // ASSERT

            Assert.IsNotNull(actual);

            loyaltyInfo = actual.Profiles.ProfileInfo.UniqueID.Where(x => x.ID_Context == "LOYALTYNUMBER");

            personName = actual.Profiles.ProfileInfo.Profile.Customer.PersonName;

            customerLoyalty = actual.Profiles.ProfileInfo.Profile.Customer.CustLoyalty;

            Assert.AreEqual(_profilePublisherRequest.MileagePlanNumber, loyaltyInfo.FirstOrDefault().ID);
            Assert.AreEqual(_profilePublisherRequest.FirstName, personName.GivenName);
            Assert.AreEqual(_profilePublisherRequest.LastName, personName.Surname);
            Assert.AreEqual("RUBY", customerLoyalty.LoyalLevelDescription);

            // DELETE PROFILE

            // ARRANGE

            _profilePublisherRequest.EventType = "TIER_DOWNGRADE_REGULAR";

            // ACT

            Thread.Sleep(20000);
            await _amadeusOperations.ProfileDelete(session, _profilePublisherRequest);
            Thread.Sleep(1000);
            actual = await _amadeusOperations.ProfileRetrieve(session, _profilePublisherRequest);

            //Signout session.
            await _amadeusOperations.SignOut(session);

            // ASSERT
            Assert.IsNull(actual);

        }

        [Test]
        public async Task AmadeusSyncTest()
        {
            // INSERT PROFILE

            // ARRANGE

            var input = JsonConvert.SerializeObject(_profilePublisherRequest);

            // ACT

            await _QueueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(input)));

            Thread.Sleep(1000);
            Session session = null;

            while (session == null)
            {
                session = await _amadeusOperations.Authenticate();
            }

            int retryCount = 10;

            AMA_ProfileReadRS actual = null;

            while (retryCount-- > 0 && actual == null)
            {
                Thread.Sleep(3000);
                actual = await _amadeusOperations.ProfileRetrieve(session, _profilePublisherRequest);
            }

            // ASSERT

            Assert.IsNotNull(actual);

            var loyaltyInfo = actual.Profiles.ProfileInfo.UniqueID.Where(x => x.ID_Context == "LOYALTYNUMBER");

            var personName = actual.Profiles.ProfileInfo.Profile.Customer.PersonName;
            var customerLoyalty = actual.Profiles.ProfileInfo.Profile.Customer.CustLoyalty;

            Assert.AreEqual(_profilePublisherRequest.MileagePlanNumber, loyaltyInfo.FirstOrDefault().ID);
            Assert.AreEqual(_profilePublisherRequest.FirstName, personName.GivenName);
            Assert.AreEqual(_profilePublisherRequest.LastName, personName.Surname);
            Assert.AreEqual("EMER", customerLoyalty.LoyalLevelDescription);

            // UPDATE PROFILE

            // ARRANGE

            _profilePublisherRequest.OneWorldTier = "RUBY";
            _profilePublisherRequest.TierId = "1";
            _profilePublisherRequest.FirstName = "UPDFIRSTNAME";
            _profilePublisherRequest.LastName = "UPDLASTNAME";
            _profilePublisherRequest.ModificationNumber = "2";

            input = JsonConvert.SerializeObject(_profilePublisherRequest);

            // ACT

            await _QueueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(input)));

            retryCount = 10;

            actual = null;

            while (retryCount-- > 0 && actual == null)
            {
                Thread.Sleep(3000);
                actual = await _amadeusOperations.ProfileRetrieve(session, _profilePublisherRequest);
            }

            // ASSERT

            Assert.IsNotNull(actual);

            loyaltyInfo = actual.Profiles.ProfileInfo.UniqueID.Where(x => x.ID_Context == "LOYALTYNUMBER");

            personName = actual.Profiles.ProfileInfo.Profile.Customer.PersonName;

            customerLoyalty = actual.Profiles.ProfileInfo.Profile.Customer.CustLoyalty;

            Assert.AreEqual(_profilePublisherRequest.MileagePlanNumber, loyaltyInfo.FirstOrDefault().ID);
            Assert.AreEqual(_profilePublisherRequest.FirstName, personName.GivenName);
            Assert.AreEqual(_profilePublisherRequest.LastName, personName.Surname);
            Assert.AreEqual("RUBY", customerLoyalty.LoyalLevelDescription);

            // DELETE PROFILE

            // ARRANGE

            _profilePublisherRequest.EventType = "TIER_DOWNGRADE_REGULAR";

            input = JsonConvert.SerializeObject(_profilePublisherRequest);

            // ACT

            await _QueueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(input)));

            actual = null;

            retryCount = 10;

            while (retryCount-- > 0)
            {
                Thread.Sleep(1000);
                actual = await _amadeusOperations.ProfileRetrieve(session, _profilePublisherRequest);
            }

            //Signout session.
            await _amadeusOperations.SignOut(session);

            // ASSERT

            Assert.IsNull(actual);
        }
       
    }
}