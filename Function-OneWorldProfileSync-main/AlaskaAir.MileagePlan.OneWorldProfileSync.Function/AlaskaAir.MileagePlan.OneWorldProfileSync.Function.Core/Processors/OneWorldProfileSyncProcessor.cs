using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Constants;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Exceptions;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Processors
{
    public class OneWorldProfileSyncProcessor
    {
        private readonly AmadeusOperations _amadeusOperations;
        private readonly IAmadeusRequestCreationHelper _helpers;
        private readonly ILogger<OneWorldProfileSyncProcessor> _logger;

        public OneWorldProfileSyncProcessor(IAmadeusRequestCreationHelper Helpers,AmadeusOperations amadeusOperations, ILogger<OneWorldProfileSyncProcessor> logger)
        {
            _amadeusOperations = amadeusOperations;
            _helpers = Helpers;
            _logger = logger;
        }

        public async Task ProcessSyncRequestAsync(ProfilePublisherRequest profilePublisherRequest, Message message, ICollector<Message> outputValidationQueue)
        {
            var session = await _amadeusOperations.Authenticate();

            if (string.IsNullOrWhiteSpace(session.SessionId))
            {
                throw new InvalidSessionException("Session ID is null or contains only white spaces.");
            }

            _logger.LogInformation("Successfully Authenticated.");

            var eventType = profilePublisherRequest.EventType.ToUpper();

            var oneWorldTier = profilePublisherRequest?.OneWorldTier?.ToUpper();

            // MEMBER_REACTIVATE should have no impact as member would be regular after reactivation
            // MEMBER_UPDATE should perform UPSERT only when oneworld tier is not empty

            if (eventType == EventTypes.TierUpgradeEventType
                || eventType == EventTypes.TierDowngradeEventType
                || ((eventType == EventTypes.MemberUpdateEventType || eventType == EventTypes.BatchUploadEventType) &&  !string.IsNullOrEmpty(oneWorldTier) && profilePublisherRequest.MemberType == "Individual"))
            {
                _logger.LogInformation($"upsert:Calling upsert operation for member number:{profilePublisherRequest.MileagePlanNumber}");

                await _amadeusOperations.ProfileUpsert(session, profilePublisherRequest);

                //Retrieve operation.
                #region
                /*
                _logger.LogInformation($"upsert:Calling Retrieve operation for member number:{profilePublisherRequest.MileagePlanNumber}");
                
                var retrieve = await _amadeusOperations.ProfileRetrieve(session, profilePublisherRequest);

                var retrieve_jsondata = new
                {
                    Mileageplannumber = retrieve.Profiles.ProfileInfo.UniqueID[2].ID,
                    FirstName = retrieve.Profiles.ProfileInfo.Profile.Customer.PersonName.GivenName,
                    LastName = retrieve.Profiles.ProfileInfo.Profile.Customer.PersonName.Surname,
                    LoyName = retrieve.Profiles.ProfileInfo.Profile.Customer.CustLoyalty.LoyalLevelDescription,
                    LoyLevel = retrieve.Profiles.ProfileInfo.Profile.Customer.CustLoyalty.LoyalLevel
                };

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(retrieve_jsondata);
                var retrieve_message = new Message(Encoding.UTF8.GetBytes(jsonString));

                //var oneWorldMappedTier = _helpers.MapToOneWorldTier(profilePublisherRequest.OneWorldTier);

                if (retrieve_jsondata.Mileageplannumber== profilePublisherRequest.MileagePlanNumber && retrieve_jsondata.FirstName== profilePublisherRequest.FirstName
                    && retrieve_jsondata.LastName== profilePublisherRequest.LastName)
                {
                    _logger.LogInformation($"upsert:Profile retrieve Match successful for member number :{profilePublisherRequest.MileagePlanNumber}");
                    ServiceBusMessageProcessor.ReQueueMessage(retrieve_message, outputValidationQueue);
                }
                else
                {
                    _logger.LogInformation($"upsert:Profile retrieve does not Match for member number :{profilePublisherRequest.MileagePlanNumber}");
                }
                */
                #endregion
            }
            else if (eventType == EventTypes.BatchRefreshEventType && profilePublisherRequest.MemberType == "Individual")
            {
                _logger.LogInformation($"Refresh:Calling Refresh operation for member number:{profilePublisherRequest.MileagePlanNumber}");

                await _amadeusOperations.ProfileRefresh(session, profilePublisherRequest);

                //Retrieve operation.
                #region
                /*
                _logger.LogInformation($"Refresh:Calling Retrieve operation for member number:{profilePublisherRequest.MileagePlanNumber}");

                var retrieve = await _amadeusOperations.ProfileRetrieve(session, profilePublisherRequest);

                var retrieve_jsondata = new
                {
                    Mileageplannumber = retrieve.Profiles.ProfileInfo.UniqueID[2].ID,
                    FirstName = retrieve.Profiles.ProfileInfo.Profile.Customer.PersonName.GivenName,
                    LastName = retrieve.Profiles.ProfileInfo.Profile.Customer.PersonName.Surname,
                    LoyName = retrieve.Profiles.ProfileInfo.Profile.Customer.CustLoyalty.LoyalLevelDescription,
                    LoyLevel = retrieve.Profiles.ProfileInfo.Profile.Customer.CustLoyalty.LoyalLevel
                };

                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(retrieve_jsondata);
                var retrieve_message = new Message(Encoding.UTF8.GetBytes(jsonString));

                //var oneWorldMappedTier = _helpers.MapToOneWorldTier(profilePublisherRequest.OneWorldTier);

                if (retrieve_jsondata.Mileageplannumber == profilePublisherRequest.MileagePlanNumber && retrieve_jsondata.FirstName == profilePublisherRequest.FirstName
                    && retrieve_jsondata.LastName == profilePublisherRequest.LastName)
                {
                    _logger.LogInformation($"Refresh:Profile retrieve Match successful for member number :{profilePublisherRequest.MileagePlanNumber}");
                    ServiceBusMessageProcessor.ReQueueMessage(retrieve_message, outputValidationQueue);
                }
                else
                {
                    _logger.LogInformation($"Refresh:Profile retrieve does not Match for member number :{profilePublisherRequest.MileagePlanNumber}");
                }
                */
                #endregion

            }
            else if (eventType == EventTypes.TierDowngradeRegularEventType
                || (eventType == EventTypes.MemberDeleteEventType && !string.IsNullOrEmpty(oneWorldTier) && profilePublisherRequest.MemberType == "Individual")
                )
            {
                await _amadeusOperations.ProfileDelete(session, profilePublisherRequest);
            }
            else
            {
                _logger.LogDebug($"{eventType} not valid for Amadeus profile sync");
            }

            await _amadeusOperations.SignOut(session);

            _logger.LogInformation("Successfully Signout.");
        }
    }
}