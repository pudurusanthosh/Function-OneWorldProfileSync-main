using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;

namespace AlaskaAir.MileagePlan.OWProfileSync.Function.UnitTests.Helpers
{
    [TestFixture]
    public class AmadeusRequestCreationHelperTests
    {
        IConfigurationRoot config = null;
        Mock<AmadeusRequestCreationHelper> amadeusRequestCreationHelperMock;

        [OneTimeSetUp]
        public void Init()
        {
            var uri = "https://nodeA3.test.webservices.amadeus.com/Some";
            Environment.SetEnvironmentVariable("amadeusPassword", "SomePassword");
            Environment.SetEnvironmentVariable("wsaTo", uri);
            Environment.SetEnvironmentVariable("amadeusBaseUri", uri);
            Environment.SetEnvironmentVariable("sourceOffice", "someSourceOffice");
            Environment.SetEnvironmentVariable("originator", "SomeOriginator");

            config = new ConfigurationBuilder()
                            .AddEnvironmentVariables()
                            .Build();
            
            amadeusRequestCreationHelperMock = new Mock<AmadeusRequestCreationHelper>(config);
        }

        [TestCase("RUBY", "RUBY", 1)]
        [TestCase("SAPPHIRE", "SAPH", 2)]
        [TestCase("EMERALD", "EMER", 3)]
        public void MapToOneWorldTier_Maps_Tiers_To_OneWorld(string oneWorldTierName, string mappedTierName, int mappedLoyaltyLevel)
        {
            amadeusRequestCreationHelperMock.CallBase = true;

            var mappedResponse = amadeusRequestCreationHelperMock.Object.MapToOneWorldTier(oneWorldTierName);

            Assert.AreEqual(mappedTierName, mappedResponse.Name);
            Assert.AreEqual(mappedLoyaltyLevel, mappedResponse.LoyaltyLevel);
        }

        [TestCase("Something", "U29tZXRoaW5n")]
        [TestCase("EncodeThis100", "RW5jb2RlVGhpczEwMA==")]
        [TestCase("EncodeThis$123", "RW5jb2RlVGhpcyQxMjM=")]
        public void Base64Encode_Encodes_Data(string rawData, string expectedEncodedData)
        {
            var encodedString = amadeusRequestCreationHelperMock.Object.Base64Encode(rawData);

            Assert.AreEqual(expectedEncodedData, encodedString);
        }

        [Test]
        public void GenerateAuthenticationRequest_Returns_Message()
        {
            var expectedResponse = amadeusRequestCreationHelperMock.Object.GenerateAuthenticationRequest();
            var encodedPassword = amadeusRequestCreationHelperMock.Object.Base64Encode(Environment.GetEnvironmentVariable("amadeusPassword"));
            Assert.IsTrue(expectedResponse.Contains(encodedPassword));
            Assert.IsTrue(expectedResponse.Contains(Environment.GetEnvironmentVariable("wsaTo")));
            Assert.IsTrue(expectedResponse.Contains(Environment.GetEnvironmentVariable("amadeusBaseUri")));
            Assert.IsTrue(expectedResponse.Contains(Environment.GetEnvironmentVariable("sourceOffice")));
            Assert.IsTrue(expectedResponse.Contains(Environment.GetEnvironmentVariable("wsaTo")));
            Assert.IsTrue(expectedResponse.Contains(Environment.GetEnvironmentVariable("originator")));
        }

        [Test]
        public void GenerateProfileAirlineUpsert_Returns_Message()
        {
            Session session = new Session
            {
                SessionId = "1ASWD",
                SecurityToken = "HELLO",
                SequenceNumber = 1
            };

            ProfilePublisherRequest profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "1",
                MileagePlanNumber = DateTime.Now.ToString("yyyyMMddHHmmss"),
                EventType = "TIER_UPGRADE",
                TierId = "1",
                TierName = "MVP",
                LastName = "EMERALD",
                FirstName = "TESTMEMBER",
                OneWorldTier = "EMERALD"
            };

            var airlineUpsertRequest = amadeusRequestCreationHelperMock.Object.GenerateProfileAirlineUpsert(session, profilePublisherRequest);

            Assert.IsTrue(airlineUpsertRequest.Contains(profilePublisherRequest.ModificationNumber));
            Assert.IsTrue(airlineUpsertRequest.Contains(profilePublisherRequest.MileagePlanNumber));
            Assert.IsTrue(airlineUpsertRequest.Contains(profilePublisherRequest.LastName));
            Assert.IsTrue(airlineUpsertRequest.Contains(profilePublisherRequest.FirstName));
            Assert.IsTrue(airlineUpsertRequest.Contains(profilePublisherRequest.OneWorldTier));
            Assert.IsTrue(airlineUpsertRequest.Contains(session.SessionId));
            Assert.IsTrue(airlineUpsertRequest.Contains(session.SecurityToken));
            Assert.IsTrue(airlineUpsertRequest.Contains(session.SequenceNumber.ToString()));
        }

        [Test]
        public void GenerateProfileAirlineDelete_Returns_Message()
        {
            Session session = new Session
            {
                SessionId = "1ASWD",
                SecurityToken = "HELLO",
                SequenceNumber = 1
            };

            ProfilePublisherRequest profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "1",
                MileagePlanNumber = DateTime.Now.ToString("yyyyMMddHHmmss"),
                EventType = "TIER_UPGRADE",
                TierId = "1",
                TierName = "MVP",
                LastName = "EMERALD",
                FirstName = "TESTMEMBER",
                OneWorldTier = "EMERALD"
            };

            var airlineDeleteRequest = amadeusRequestCreationHelperMock.Object.GenerateProfileDelete(session, profilePublisherRequest);

            Assert.IsTrue(airlineDeleteRequest.Contains(profilePublisherRequest.MileagePlanNumber));
            Assert.IsTrue(airlineDeleteRequest.Contains(session.SessionId));
            Assert.IsTrue(airlineDeleteRequest.Contains(session.SecurityToken));
            Assert.IsTrue(airlineDeleteRequest.Contains(session.SequenceNumber.ToString()));
        }

        [Test]
        public void GenerateSignoutRequest_Returns_Message()
        {
            Session session = new Session
            {
                SessionId = "1ASWD",
                SecurityToken = "HELLO",
                SequenceNumber = 1
            };

            var expectedResponse = amadeusRequestCreationHelperMock.Object.GenerateSignoutRequest(session);
            Assert.IsTrue(expectedResponse.Contains(session.SessionId));
            Assert.IsTrue(expectedResponse.Contains(session.SequenceNumber.ToString()));
            Assert.IsTrue(expectedResponse.Contains(session.SecurityToken));
        }

        [Test]
        public void GenerateProfileAirlineRetrieve_Returns_Message()
        {
            Session session = new Session
            {
                SessionId = "1ASWD",
                SecurityToken = "HELLO",
                SequenceNumber = 1
            };

            ProfilePublisherRequest profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "1",
                MileagePlanNumber = DateTime.Now.ToString("yyyyMMddHHmmss"),
                EventType = "TIER_UPGRADE",
                TierId = "1",
                TierName = "MVP",
                LastName = "EMERALD",
                FirstName = "TESTMEMBER",
                OneWorldTier = "EMERALD"
            };

            var airlineRetrieveRequest = amadeusRequestCreationHelperMock.Object.GenerateProfileRetrieveRequest(session, profilePublisherRequest);

            Assert.IsTrue(airlineRetrieveRequest.Contains(profilePublisherRequest.MileagePlanNumber));
            Assert.IsTrue(airlineRetrieveRequest.Contains(session.SessionId));
            Assert.IsTrue(airlineRetrieveRequest.Contains(session.SecurityToken));
            Assert.IsTrue(airlineRetrieveRequest.Contains(session.SequenceNumber.ToString()));
        }
    }
}