using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;

namespace AlaskaAir.MileagePlan.OWProfileSync.Function.UnitTests.Helpers
{
    [TestFixture]
    public class ValidatorTests
    {
        Validator validator;

        [OneTimeSetUp]
        public void Init()
        {
            var iLoggerMock = new Mock<ILogger<Validator>>();

            validator = new Validator(iLoggerMock.Object);
        }

        [Test]
        public void ValidateRequest_Given_ValidRequest_Return_True()
        {

            var profilePublisherRequest = new ProfilePublisherRequest
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

            var response = validator.ValidateRequest(profilePublisherRequest);

            Assert.IsTrue(response);
        }

        [Test]
        public void ValidateRequest_Given_InValidRequest_Return_False()
        {

            var profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "1",
                MileagePlanNumber = string.Empty,
                EventType = "TIER_UPGRADE",
                TierId = "1",
                TierName = "MVP",
                LastName = "EMERALD",
                FirstName = "TESTMEMBER",
                OneWorldTier = "EMERALD"
            };

            var response = validator.ValidateRequest(profilePublisherRequest);

            Assert.IsFalse(response);

            profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "1",
                MileagePlanNumber = "123",
                EventType = "TIER_UPGRADE",
                TierId = "1",
                TierName = "MVP",
                LastName = "",
                FirstName = "TESTMEMBER",
                OneWorldTier = "EMERALD"
            };

            response = validator.ValidateRequest(profilePublisherRequest);

            Assert.IsFalse(response);

            profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "1",
                MileagePlanNumber = "123",
                EventType = "TIER_UPGRADE",
                TierId = "1",
                TierName = "MVP",
                LastName = "LASTNAME",
                FirstName = string.Empty,
                OneWorldTier = "EMERALD"
            };

            response = validator.ValidateRequest(profilePublisherRequest);

            Assert.IsFalse(response);
        }
    }
}
