using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Exceptions;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Processors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace AlaskaAir.MileagePlan.OWProfileSync.Function.UnitTests.Processors
{
    [TestFixture]
    public class AmadeusOperationsTests
    {
        private MockRepository mockRepository;
        private Mock<ILogger<AmadeusOperations>> iLoggerMock;
        private Mock<IHttpRequestHelper> iHttpRequestHelperMock;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            iLoggerMock = mockRepository.Create<ILogger<AmadeusOperations>>(MockBehavior.Default);

            iHttpRequestHelperMock = mockRepository.Create<IHttpRequestHelper>();
        }

        [TearDown]
        public void TearDown()
        {
            mockRepository.VerifyAll();
        }

        [Test]
        public async Task Auth_Success_Expect_SessionObject()
        {
            var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
            Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
            Environment.SetEnvironmentVariable("wsaTo", uri);
            Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);

            var config = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .Build();

            iHttpRequestHelperMock.Setup(x => x.PostHttpRequest<AuthResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResponse
                {
                    Header = new EnvelopeHeader
                    {
                        Session = new Session
                        {
                            SessionId = "111111",
                            SequenceNumber = 1,
                            SecurityToken = "123456"
                        }
                    },
                    Body = new AuthEnvelopeBody
                    {
                        Security_AuthenticateReply = new Security_AuthenticateReply
                        {
                            processStatus = new Security_AuthenticateReplyProcessStatus
                            {
                                statusCode = "P"
                            }
                        }
                    }
                });

            AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

            AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, iLoggerMock.Object);

            var actual = await amadeusOpertions.Authenticate();

            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual("111111", actual.SessionId);
            Assert.AreEqual(1, actual.SequenceNumber);
            Assert.AreEqual("123456", actual.SecurityToken);
        }

        [Test]
        public void Auth_Failed_Expect_UnAuthorizeException()
        {
            var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
            Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
            Environment.SetEnvironmentVariable("wsaTo", uri);
            Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);

            var config = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .Build();

            iHttpRequestHelperMock.Setup(x => x.PostHttpRequest<AuthResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResponse
                {
                    Body = new AuthEnvelopeBody
                    {
                        Security_AuthenticateReply = new Security_AuthenticateReply
                        {
                            errorSection = new Security_AuthenticateReplyErrorSection
                            {
                                applicationError = new Security_AuthenticateReplyErrorSectionApplicationError
                                {
                                    errorDetails = new Security_AuthenticateReplyErrorSectionApplicationErrorErrorDetails
                                    {
                                        errorCode = 16002,
                                        errorCategory = "EC",
                                        errorCodeOwner = "LSS"
                                    }
                                },
                                interactiveFreeText = new Security_AuthenticateReplyErrorSectionInteractiveFreeText
                                {
                                    freeText = new[] {"freeText"}
                                }
                            }
                        }
                    }
                });

            AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

            AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, iLoggerMock.Object);

            Assert.ThrowsAsync<UnauthorizedAccessException>(() => amadeusOpertions.Authenticate());
        }

        //[Test]
        //public void Auth_Non_200_Expect_AmadeusRequestException()
        //{
        //    var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
        //    Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
        //    Environment.SetEnvironmentVariable("wsaTo", uri);
        //    Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);


        //    var config = new ConfigurationBuilder()
        //            .AddEnvironmentVariables()
        //            .Build();

        //    //iHttpRequestHelperMock.Setup(x => x.PostHttpRequest<AuthResponse>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //    //    .ReturnsAsync(new HttpResponseMessage
        //    //    {
        //    //        StatusCode = HttpStatusCode.InternalServerError
        //    //    });

        //    AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

        //    AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, config, iLoggerMock.Object);

        //    Assert.ThrowsAsync<AmadeusRequestException>(() => amadeusOpertions.Authenticate());
        //}

        [Test]
        public async Task Profile_Upsert_Success_Expect_True()
        {
            var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
            Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
            Environment.SetEnvironmentVariable("wsaTo", uri);
            Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);



            var _profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "2",
                MileagePlanNumber = "1020",
                TierId = "1",
                TierName = "EMER",
                LastName = "Emerald",
                FirstName = "TestMember",
                OneWorldTier = "RUBY"
            };

            var config = new ConfigurationBuilder()
           .AddEnvironmentVariables()
           .Build();


            iHttpRequestHelperMock.Setup(x => x.PostHttpRequest<ProfileUpsertResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ProfileUpsertResponse
                {
                    Body = new ProfileUpsertEnvelopeBody
                    {
                        AMA_UpdateRS = new AMA_UpdateRS
                        {
                            Success = ""
                        }
                    }
                });


            AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

            AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, iLoggerMock.Object);

            var session = new Session
            {
                SessionId = "11111",
                SequenceNumber = 1,
                SecurityToken = "123456",
            };

            var actual = await amadeusOpertions.ProfileUpsert(session, _profilePublisherRequest);

            Assert.IsNotNull(actual);

        }

        //[Test]
        //public void Profile_Upsert_Non_200_Expect_AmadeusRequestException()
        //{
        //    var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
        //    Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
        //    Environment.SetEnvironmentVariable("wsaTo", uri);
        //    Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);



        //    var _profilePublisherRequest = new ProfilePublisherRequest
        //    {
        //        ModificationNumber = "2",
        //        MileagePlanNumber = "1020",
        //        TierId = "1",
        //        TierName = "EMER",
        //        LastName = "Emerald",
        //        FirstName = "TestMember"
        //    };

        //    var config = new ConfigurationBuilder()
        //   .AddEnvironmentVariables()
        //   .Build();

        //    iHttpRequestHelperMock.Setup(x => x.PostHttpRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //       .ReturnsAsync(new HttpResponseMessage
        //       {
        //           StatusCode = HttpStatusCode.InternalServerError
        //       });

        //    AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

        //    AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, config, iLoggerMock.Object);

        //    var _session = new Session
        //    {
        //        SessionId = "11111",
        //        SequenceNumber = 1,
        //        SecurityToken = "123456"
        //    };

        //    Assert.ThrowsAsync<AmadeusRequestException>(() => amadeusOpertions.ProfileUpsert(_session, _profilePublisherRequest));

        //}

        [Test]
        public void Profile_Upsert_Error_Expect_ProfileUpsertException()
        {
            var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
            Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
            Environment.SetEnvironmentVariable("wsaTo", uri);
            Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);

            var _profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "2",
                MileagePlanNumber = "1020",
                TierId = "1",
                TierName = "EMER",
                LastName = "Emerald",
                FirstName = "TestMember",
                OneWorldTier = "SAPPHIRE"
            };

            var config = new ConfigurationBuilder()
                           .AddEnvironmentVariables()
                           .Build();


            iHttpRequestHelperMock.Setup(x => x.PostHttpRequest<ProfileUpsertResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ProfileUpsertResponse
                {
                    Body = new ProfileUpsertEnvelopeBody
                    {
                        AMA_UpdateRS = new AMA_UpdateRS
                        {
                            Errors = new AMA_UpdateRSErrors
                            {
                                Error = new AMA_UpdateRSErrorsError
                                {
                                    Type = 21,
                                    ShortText = "INVALID VERSION NUMBER",
                                    Code = "11.IAG",
                                    Value = "Profile out of synchro."
                                }
                            }
                        }
                    }
                });


            AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

            AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, iLoggerMock.Object);

            var session = new Session
            {
                SessionId = "11111",
                SequenceNumber = 1,
                SecurityToken = "123456"
            };

            Assert.ThrowsAsync<ProfileUpsertException>(() => amadeusOpertions.ProfileUpsert(session, _profilePublisherRequest));
        }

        [Test]
        public async Task Profile_Refresh_Success_Expect_True()
        {
            var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
            Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
            Environment.SetEnvironmentVariable("wsaTo", uri);
            Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);

            var _profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "2",
                MileagePlanNumber = "1020",
                TierId = "1",
                TierName = "EMER",
                LastName = "Emerald",
                FirstName = "TestMember",
                OneWorldTier = "RUBY"
            };

            var config = new ConfigurationBuilder()
           .AddEnvironmentVariables()
           .Build();


            iHttpRequestHelperMock.Setup(x => x.PostHttpRequest<ProfileUpsertResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ProfileUpsertResponse
                {
                    Body = new ProfileUpsertEnvelopeBody
                    {
                        AMA_UpdateRS = new AMA_UpdateRS
                        {
                            Success = ""
                        }
                    }
                });


            AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

            AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, iLoggerMock.Object);

            var session = new Session
            {
                SessionId = "11111",
                SequenceNumber = 1,
                SecurityToken = "123456",
            };

            var actual = await amadeusOpertions.ProfileRefresh(session, _profilePublisherRequest);

            Assert.IsNotNull(actual);

        }

        [Test]
        public void Profile_Refresh_Error_Expect_ProfileUpsertException()
        {
            var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
            Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
            Environment.SetEnvironmentVariable("wsaTo", uri);
            Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);

            var _profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "null",
                MileagePlanNumber = "1020",
                TierId = "1",
                TierName = "EMER",
                LastName = "Emerald",
                FirstName = "TestMember",
                OneWorldTier = "SAPPHIRE"
            };

            var config = new ConfigurationBuilder()
                           .AddEnvironmentVariables()
                           .Build();


            iHttpRequestHelperMock.Setup(x => x.PostHttpRequest<ProfileUpsertResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ProfileUpsertResponse
                {
                    Body = new ProfileUpsertEnvelopeBody
                    {
                        AMA_UpdateRS = new AMA_UpdateRS
                        {
                            Errors = new AMA_UpdateRSErrors
                            {
                                Error = new AMA_UpdateRSErrorsError
                                {
                                    Type = 21,
                                    ShortText = "SEQUENCE NUMBER IS MISSING",
                                    Code = "32.GCC",
                                    Value = "Missing Remote Profile and Remote Mileage Sequence Number."
                                }
                            }
                        }
                    }
                });


            AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

            AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, iLoggerMock.Object);

            var session = new Session
            {
                SessionId = "11111",
                SequenceNumber = 1,
                SecurityToken = "123456"
            };

            Assert.ThrowsAsync<ProfileUpsertException>(() => amadeusOpertions.ProfileRefresh(session, _profilePublisherRequest));
        }

        //[Test]
        //public async Task TestSignout()
        //{
        //    var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
        //    System.Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
        //    System.Environment.SetEnvironmentVariable("wsaTo", uri);
        //    System.Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);
        //    System.Environment.SetEnvironmentVariable("AmadeusAutenticateSoapAction", "http://webservices.amadeus.com/VLSSLQ_06_1_1A");
        //    System.Environment.SetEnvironmentVariable("AmadeusSignoutSoapAction", "http://webservices.amadeus.com/VLSSOQ_04_1_1A");
        //    System.Environment.SetEnvironmentVariable("AmadeusProfileUpsertAction", "http://webservices.amadeus.com/Profile_AirlineUpdate_12.2");
        //    System.Environment.SetEnvironmentVariable("AmadeusProfileRetrieveSoapAction", "http://webservices.amadeus.com/Profile_AirlineRetrieve_12.2");
        //    System.Environment.SetEnvironmentVariable("AmadeusProfileDeleteAction", "http://webservices.amadeus.com/Profile_AirlineDelete_12.2");

        //    var profilePublisherRequest = new ProfilePublisherRequest {
        //        ModificationNumber = "1",
        //        MileagePlanNumber = "1037",
        //        EventType = "UPSERT",
        //        TierId = "1",
        //        TierName = "EMER",
        //        LastName = "Emerald",
        //        FirstName = "TestMember",
        //    };
        //    Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
        //    Environment.SetEnvironmentVariable("wsaTo", uri);
        //    Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);




        //    var config = new ConfigurationBuilder()
        //            .AddEnvironmentVariables()
        //            .Build();

        //    var mock = new Mock<IHttpClientFactory>();

        //    mock.Setup(x => x.CreateClient(It.IsAny<string>()))
        //        .Returns(new HttpClient());

        //    iHttpRequestHelperMock httpRequestHelper = new HttpRequestHelper(mock.Object, config);

        //    AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

        //    AmadeusOperations amadeusOpertions = new AmadeusOperations(httpRequestHelper, generateAmadeusRequests, config, iLoggerMock.Object);

        //    var session = await amadeusOpertions.Authenticate();

        //    Assert.IsNotNull(session);

        //    var newProfile = await amadeusOpertions.ProfileUpsert(session, profilePublisherRequest);

        //    Assert.IsNotNull(newProfile);
        //    Assert.AreEqual(profilePublisherRequest.MileagePlanNumber, newProfile.ExternalID.ID.ToString());

        //    var existingProfile = await amadeusOpertions.ProfileRetrieve(session, profilePublisherRequest);
        //    var loyaltyNumber = existingProfile.Profiles.ProfileInfo.UniqueID.FirstOrDefault(id => id.ID_Context == "LOYALTYNUMBER");
        //    Assert.IsNotNull(existingProfile);
        //    Assert.IsNotNull(loyaltyNumber);
        //    Assert.AreEqual(profilePublisherRequest.MileagePlanNumber, loyaltyNumber.ID);

        //    await amadeusOpertions.ProfileDelete(session, profilePublisherRequest);
        //    Assert.ThrowsAsync<ProfileRetrieveException>(() => amadeusOpertions.ProfileRetrieve(session, profilePublisherRequest));

        //    var successSignOut = await amadeusOpertions.SignOut(session);
        //    Assert.IsTrue(successSignOut);
        //}

        [Test]
        public async Task ProfileDelete_Success_Expect_True()
        {
            var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
            Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
            Environment.SetEnvironmentVariable("wsaTo", uri);
            Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);



            var _profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "2",
                MileagePlanNumber = "1020",
                TierId = "1",
                TierName = "EMER",
                LastName = "Emerald",
                FirstName = "TestMember"
            };

            var config = new ConfigurationBuilder()
           .AddEnvironmentVariables()
           .Build();


            iHttpRequestHelperMock.Setup(x => x.PostHttpRequest<ProfileDeleteResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ProfileDeleteResponse
                {
                    Body = new ProfileDeleteEnvelopeBody
                    {
                        AMA_DeleteRS = new AMA_DeleteRS
                        {

                        }
                    }
                });

            AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

            AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, iLoggerMock.Object);

            var session = new Session
            {
                SessionId = "11111",
                SequenceNumber = 1,
                SecurityToken = "123456",
            };

            var actual = await amadeusOpertions.ProfileDelete(session, _profilePublisherRequest);

            Assert.IsNotNull(actual);
        }

        //[Test]
        //public void ProfileDelete_Non_200_Expect_AmadeusRequestException()
        //{
        //    var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
        //    Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
        //    Environment.SetEnvironmentVariable("wsaTo", uri);
        //    Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);



        //    var _profilePublisherRequest = new ProfilePublisherRequest
        //    {
        //        ModificationNumber = "2",
        //        MileagePlanNumber = "1020",
        //        TierId = "1",
        //        TierName = "EMER",
        //        LastName = "Emerald",
        //        FirstName = "TestMember"
        //    };

        //    var config = new ConfigurationBuilder()
        //   .AddEnvironmentVariables()
        //   .Build();

        //    iHttpRequestHelperMock.Setup(x => x.PostHttpRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //       .ReturnsAsync(new HttpResponseMessage
        //       {
        //           StatusCode = HttpStatusCode.InternalServerError
        //       });

        //    AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

        //    AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, config, iLoggerMock.Object);

        //    var _session = new Session
        //    {
        //        SessionId = "11111",
        //        SequenceNumber = 1,
        //        SecurityToken = "123456"
        //    };

        //    Assert.ThrowsAsync<AmadeusRequestException>(() => amadeusOpertions.ProfileDelete(_session, _profilePublisherRequest));
        //}

        [Test]
        public void ProfileDelete_Error_Expect_ProfileDeleteException()
        {
            var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
            Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
            Environment.SetEnvironmentVariable("wsaTo", uri);
            Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);



            var _profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "2",
                MileagePlanNumber = "1020",
                TierId = "1",
                TierName = "EMER",
                LastName = "Emerald",
                FirstName = "TestMember"
            };

            var config = new ConfigurationBuilder()
           .AddEnvironmentVariables()
           .Build();


            iHttpRequestHelperMock.Setup(x => x.PostHttpRequest<ProfileDeleteResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ProfileDeleteResponse
                {
                    Body = new ProfileDeleteEnvelopeBody
                    {
                        AMA_DeleteRS = new AMA_DeleteRS
                        {
                            Errors = new AMA_UpdateRSErrors
                            {
                                Error = new AMA_UpdateRSErrorsError
                                {
                                    Type = 21,
                                    ShortText = "FREQUENT FLYER NUMBER NOT FOUND",
                                    Code = "9.CFD"
                                }
                            }
                        }
                    }
                });
                
            AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

            AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, iLoggerMock.Object);

            var session = new Session
            {
                SessionId = "11111",
                SequenceNumber = 1,
                SecurityToken = "123456"
            };

            Assert.ThrowsAsync<ProfileDeleteException>(() => amadeusOpertions.ProfileDelete(session, _profilePublisherRequest));
        }

        [Test]
        public async Task Signout_Success_Expect_True()
        {
            var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
            Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
            Environment.SetEnvironmentVariable("wsaTo", uri);
            Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);


            var config = new ConfigurationBuilder()
               .AddEnvironmentVariables()
               .Build();


            iHttpRequestHelperMock.Setup(x => x.PostHttpRequest<AuthResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AuthResponse
                {
                    Body = new AuthEnvelopeBody
                    {
                        Security_SignOutReply = new Security_SignOutReply
                        {
                            processStatus = new Security_SignOutReplyProcessStatus
                            {
                                statusCode = "P"
                            }
                        }
                    }
                });
                
            AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

            AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, iLoggerMock.Object);

            var session = new Session
            {
                SessionId = "11111",
                SequenceNumber = 1,
                SecurityToken = "123456",
            };

            bool success = await amadeusOpertions.SignOut(session);

            Assert.IsTrue(success);
        }

        //[Test]
        //public void Signout_Non_200_Expect_AmadeusRequestException()
        //{
        //    var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
        //    Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
        //    Environment.SetEnvironmentVariable("wsaTo", uri);
        //    Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);


        //    var config = new ConfigurationBuilder()
        //   .AddEnvironmentVariables()
        //   .Build();

        //    iHttpRequestHelperMock.Setup(x => x.PostHttpRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //       .ReturnsAsync(new HttpResponseMessage
        //       {
        //           StatusCode = HttpStatusCode.InternalServerError
        //       });

        //    AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

        //    AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, config, iLoggerMock.Object);

        //    var session = new Session
        //    {
        //        SessionId = "11111",
        //        SequenceNumber = 1,
        //        SecurityToken = "123456"
        //    };

        //    Assert.ThrowsAsync<AmadeusRequestException>(() => amadeusOpertions.SignOut(session));

        // }

        [Test]
        public async Task Profile_Retrieve_Success_Expect_True()
        {
            var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
            System.Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
            System.Environment.SetEnvironmentVariable("wsaTo", uri);
            System.Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);
            System.Environment.SetEnvironmentVariable("AmadeusProfileRetrieveSoapAction", "http://webservices.amadeus.com/Profile_AirlineRetrieve_12.2");

            var _profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "2",
                MileagePlanNumber = "1020",
                TierId = "1",
                TierName = "EMER",
                LastName = "Emerald",
                FirstName = "TestMember"
            };

            var config = new ConfigurationBuilder()
           .AddEnvironmentVariables()
           .Build();


            iHttpRequestHelperMock.Setup(x => x.PostHttpRequest<AmadeusProfileRetrieveResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AmadeusProfileRetrieveResponse
                {
                    Body = new AmedeusProfileEnvelopeBody
                    {
                        AMA_ProfileReadRS = new AMA_ProfileReadRS
                        {
                            Success = "P"
                        }
                    }
                });
            
            AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

            AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, iLoggerMock.Object);

            var session = new Session
            {
                SessionId = "11111",
                SequenceNumber = 1,
                SecurityToken = "123456",
            };

            var actual = await amadeusOpertions.ProfileRetrieve(session, _profilePublisherRequest);

            Assert.IsNotNull(actual);

        }

        //[Test]
        //public void Profile_Retrieve_Non_200_Expect_AmadeusRequestException()
        //{
        //    var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
        //    System.Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
        //    System.Environment.SetEnvironmentVariable("wsaTo", uri);
        //    System.Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);
        //    System.Environment.SetEnvironmentVariable("AmadeusAutenticateSoapAction", "http://webservices.amadeus.com/VLSSLQ_06_1_1A");
        //    System.Environment.SetEnvironmentVariable("AmadeusProfileUpsertAction", "http://webservices.amadeus.com/Profile_AirlineUpdate_12.2");

        //    var _profilePublisherRequest = new ProfilePublisherRequest();
        //    _profilePublisherRequest.ModificationNumber = "2";
        //    _profilePublisherRequest.MileagePlanNumber = "1020";
        //    _profilePublisherRequest.TierId = "1";
        //    _profilePublisherRequest.TierName = "EMER";
        //    _profilePublisherRequest.LastName = "Emerald";
        //    _profilePublisherRequest.FirstName = "TestMember";

        //    var config = new ConfigurationBuilder()
        //   .AddEnvironmentVariables()
        //   .Build();

        //    iHttpRequestHelperMock.Setup(x => x.PostHttpRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //       .ReturnsAsync(new HttpResponseMessage
        //       {
        //           StatusCode = HttpStatusCode.InternalServerError
        //       });

        //    AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

        //    AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, iLoggerMock.Object);

        //    var _session = new Session();
        //    _session.SessionId = "11111";
        //    _session.SequenceNumber = 1;
        //    _session.SecurityToken = "123456";

        //    Assert.ThrowsAsync<AmadeusRequestException>(() => amadeusOpertions.ProfileRetrieve(_session, _profilePublisherRequest));

        //}

        [Test]
        public void Profile_Retrieve_Error_Expect_ProfileRetrieveException()
        {
            var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
            System.Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
            System.Environment.SetEnvironmentVariable("wsaTo", uri);
            System.Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);
            System.Environment.SetEnvironmentVariable("AmadeusAutenticateSoapAction", "http://webservices.amadeus.com/VLSSLQ_06_1_1A");
            System.Environment.SetEnvironmentVariable("AmadeusProfileUpsertAction", "http://webservices.amadeus.com/Profile_AirlineUpdate_12.2");

            var _profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "2",
                MileagePlanNumber = "1020",
                TierId = "1",
                TierName = "EMER",
                LastName = "Emerald",
                FirstName = "TestMember"
            };

            var config = new ConfigurationBuilder()
           .AddEnvironmentVariables()
           .Build();


            iHttpRequestHelperMock.Setup(x => x.PostHttpRequest<AmadeusProfileRetrieveResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AmadeusProfileRetrieveResponse
                {
                    Body = new AmedeusProfileEnvelopeBody
                    {
                        AMA_ProfileReadRS = new AMA_ProfileReadRS
                        {
                            Errors = new AMA_ProfileReadRSErrors
                            {
                                Error = new AMA_ProfileReadRSErrorsError
                                {
                                    Type = 21,
                                    ShortText = "MANDATORY ITEMS MISSING",
                                    Code = "11.GBF"
                                }
                            }
                        }
                    }
                });

            AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

            AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, iLoggerMock.Object);

            var session = new Session
            {
                SessionId = "11111",
                SequenceNumber = 1,
                SecurityToken = "123456"
            };

            Assert.ThrowsAsync<ProfileRetrieveException>(() => amadeusOpertions.ProfileRetrieve(session, _profilePublisherRequest));
        }

        [Test]
        public async Task Profile_Retrieve_NotFound_Expect_Null()
        {
            var uri = "https://nodeA3.test.webservices.amadeus.com/1ASIWPASAS";
            System.Environment.SetEnvironmentVariable("AmadeusPassword", "NewOne100");
            System.Environment.SetEnvironmentVariable("wsaTo", uri);
            System.Environment.SetEnvironmentVariable("AmadeusBaseUri", uri);
            System.Environment.SetEnvironmentVariable("AmadeusAutenticateSoapAction", "http://webservices.amadeus.com/VLSSLQ_06_1_1A");
            System.Environment.SetEnvironmentVariable("AmadeusProfileUpsertAction", "http://webservices.amadeus.com/Profile_AirlineUpdate_12.2");

            var _profilePublisherRequest = new ProfilePublisherRequest
            {
                ModificationNumber = "2",
                MileagePlanNumber = "1020",
                TierId = "1",
                TierName = "EMER",
                LastName = "Emerald",
                FirstName = "TestMember"
            };

            var config = new ConfigurationBuilder()
           .AddEnvironmentVariables()
           .Build();


            iHttpRequestHelperMock.Setup(x => x.PostHttpRequest<AmadeusProfileRetrieveResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new AmadeusProfileRetrieveResponse
                {
                    Body = new AmedeusProfileEnvelopeBody
                    {
                        AMA_ProfileReadRS = new AMA_ProfileReadRS
                        {
                            Errors = new AMA_ProfileReadRSErrors
                            {
                                Error = new AMA_ProfileReadRSErrorsError
                                {
                                    Type = 21,
                                    ShortText = "FREQUENT FLYER NUMBER NOT FOUND",
                                    Code = "11.GBF"
                                }
                            }
                        }
                    }
                });

            AmadeusRequestCreationHelper generateAmadeusRequests = new AmadeusRequestCreationHelper(config);

            AmadeusOperations amadeusOpertions = new AmadeusOperations(iHttpRequestHelperMock.Object, generateAmadeusRequests, iLoggerMock.Object);

            var session = new Session
            {
                SessionId = "11111",
                SequenceNumber = 1,
                SecurityToken = "123456"
            };

            var actual = await amadeusOpertions.ProfileRetrieve(session, _profilePublisherRequest);

            Assert.IsNull(actual);
        }
    }
}
