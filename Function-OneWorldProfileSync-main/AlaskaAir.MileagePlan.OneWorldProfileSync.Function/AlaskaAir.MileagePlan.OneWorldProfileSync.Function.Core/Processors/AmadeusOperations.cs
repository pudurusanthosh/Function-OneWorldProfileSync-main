using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Exceptions;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Processors
{
    public class AmadeusOperations
    {
        private readonly IAmadeusRequestCreationHelper _amadeusRequestCreationHelper;
        private readonly IHttpRequestHelper _httpRequestHelper;
        private const string _amadeusProfileRetrieveSoapAction = "http://webservices.amadeus.com/Profile_AirlineRetrieve_12.2";
        private const string _amadeusAutenticateSoapAction = "http://webservices.amadeus.com/VLSSLQ_06_1_1A";
        private const string _amadeusProfileUpsertSoapAction = "http://webservices.amadeus.com/Profile_AirlineUpdate_12.2";
        private const string _amadeusProfileDeleteSoapAction = "http://webservices.amadeus.com/Profile_AirlineDelete_12.2";
        private const string _amadeusSignoutSoapAction = "http://webservices.amadeus.com/VLSSOQ_04_1_1A";
        private readonly ILogger<AmadeusOperations> _logger;

        public AmadeusOperations(IHttpRequestHelper httpRequestHelper, IAmadeusRequestCreationHelper amadeusRequestCreationHelper, ILogger<AmadeusOperations> logger)
        {
            _amadeusRequestCreationHelper = amadeusRequestCreationHelper;
            _httpRequestHelper = httpRequestHelper;
            _logger = logger;
        }

        public async Task<Session> Authenticate()
        {
            _logger.LogInformation("Authenticating with Amadeus");

            var req = _amadeusRequestCreationHelper.GenerateAuthenticationRequest();

            var desrializedResponse = await _httpRequestHelper.PostHttpRequest<AuthResponse>(req, _amadeusAutenticateSoapAction);

            if (desrializedResponse.Body.Security_AuthenticateReply.errorSection != default)
            {
                var errMsg = string.Join("", desrializedResponse.Body.Security_AuthenticateReply.errorSection.interactiveFreeText.freeText);
                _logger.LogError($"Error Authenticating. ERROR:{errMsg}");
                throw new UnauthorizedAccessException(errMsg);
            }

            return desrializedResponse.Header.Session;
        }

        public async Task<AMA_UpdateRS> ProfileUpsert(Session session, ProfilePublisherRequest profilePublisherRequest)
        {
            _logger.LogInformation("Upserting Amadeus Profile");

            var req = _amadeusRequestCreationHelper.GenerateProfileAirlineUpsert(session, profilePublisherRequest);

            var desrializedResponse = await _httpRequestHelper.PostHttpRequest<ProfileUpsertResponse>(req, _amadeusProfileUpsertSoapAction);

            if (desrializedResponse.Body.AMA_UpdateRS.Errors != default)
            {
                var errMsg = desrializedResponse.Body.AMA_UpdateRS.Errors.Error.ShortText;
                _logger.LogError($"Error Profile Upsert call. ERROR:{errMsg}");
                throw new ProfileUpsertException(errMsg);
            }

            _logger.LogInformation("Successfully Upserted Amadeus Profile");

            return desrializedResponse.Body.AMA_UpdateRS;
        }

        public async Task<AMA_UpdateRS> ProfileRefresh(Session session, ProfilePublisherRequest profilePublisherRequest)
        {
            _logger.LogInformation("Refresh Amadeus Profile with Refresh operation");

            var req = _amadeusRequestCreationHelper.GenerateProfileAirlineRefresh(session, profilePublisherRequest);

            var desrializedResponse = await _httpRequestHelper.PostHttpRequest<ProfileUpsertResponse>(req, _amadeusProfileUpsertSoapAction);

            if (desrializedResponse.Body.AMA_UpdateRS.Errors != default)
            {
                var errMsg = desrializedResponse.Body.AMA_UpdateRS.Errors.Error.ShortText;
                _logger.LogError($"Error Profile Refresh call. ERROR:{errMsg}");
                throw new ProfileUpsertException(errMsg);
            }

            _logger.LogInformation("Successfully Refreshed Amadeus Profile");

            return desrializedResponse.Body.AMA_UpdateRS;
        }

        public async Task<AMA_DeleteRS> ProfileDelete(Session session, ProfilePublisherRequest profilePublisherRequest)
        {
            _logger.LogInformation("Deleting Amadeus Profile");

            var req = _amadeusRequestCreationHelper.GenerateProfileDelete(session, profilePublisherRequest);

            var desrializedResponse = await _httpRequestHelper.PostHttpRequest<ProfileDeleteResponse>(req, _amadeusProfileDeleteSoapAction);

            if (desrializedResponse.Body.AMA_DeleteRS.Errors != default)
            {
                var errMsg = desrializedResponse.Body.AMA_DeleteRS.Errors.Error.ShortText;
                _logger.LogError($"Error Profile Delete call. ERROR:{errMsg}");
                throw new ProfileDeleteException(errMsg);
            }

            _logger.LogInformation("Successfully Deleted Amadeus Profile");

            return desrializedResponse.Body.AMA_DeleteRS;
        }
        
        public async Task<bool> SignOut(Session session)
        {
            _logger.LogInformation("SignOut from Amadeus");

            var req = _amadeusRequestCreationHelper.GenerateSignoutRequest(session);

            var desrializedResponse = await _httpRequestHelper.PostHttpRequest<AuthResponse>(req, _amadeusSignoutSoapAction);

            _logger.LogInformation("Successfully SignedOut from Amadeus");

            return desrializedResponse.Body.Security_SignOutReply != null;
        }

        public async Task<AMA_ProfileReadRS> ProfileRetrieve(Session session, ProfilePublisherRequest profilePublisherRequest)
        {
            var req = _amadeusRequestCreationHelper.GenerateProfileRetrieveRequest(session, profilePublisherRequest);

            var desrializedResponse = await _httpRequestHelper.PostHttpRequest<AmadeusProfileRetrieveResponse>(req, _amadeusProfileRetrieveSoapAction);

            var isError = desrializedResponse.Body?.AMA_ProfileReadRS?.Errors != default;

            var notFound = desrializedResponse.Body?.AMA_ProfileReadRS?.Errors?.Error?.ShortText == "FREQUENT FLYER NUMBER NOT FOUND";

            if (isError && !notFound)
            {
                var errMsg = desrializedResponse.Body.AMA_ProfileReadRS.Errors.Error.ShortText;
                _logger.LogError($"Error Profile Fetch call. ERROR:{errMsg}");
                throw new ProfileRetrieveException(errMsg);
            }

            return notFound ? default : desrializedResponse.Body.AMA_ProfileReadRS;
        }
    }
}