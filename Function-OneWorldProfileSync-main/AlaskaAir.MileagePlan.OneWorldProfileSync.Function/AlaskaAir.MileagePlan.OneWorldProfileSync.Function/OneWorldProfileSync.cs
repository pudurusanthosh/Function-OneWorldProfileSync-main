using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Exceptions;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Processors;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function
{
    public class OneWorldProfileSync
    {
        private readonly ILogger<OneWorldProfileSync> _logger;
        private readonly OneWorldProfileSyncProcessor _OneWorldProfileSyncProcessor;
        private readonly IValidator _validator;

        public OneWorldProfileSync(ILogger<OneWorldProfileSync> logger, OneWorldProfileSyncProcessor oneWorldProfileSyncProcessor, IValidator validator)
        {
            _logger = logger;
            _OneWorldProfileSyncProcessor = oneWorldProfileSyncProcessor;
            _validator = validator;
        }

        [FunctionName("OneWorldProfileSync")]
        public async Task Run([ServiceBusTrigger("%MemberProfileUpdateQueue%", Connection = "ServiceBusConnectionString")]Message rawMessage,
            [ServiceBus("%MemberProfileUpdateQueue%", Connection = "ServiceBusConnectionString")] ICollector<Message> outputSbQueue,
            [ServiceBus("%AmadeusCountQueue%", Connection = "ServiceBusConnectionString")] ICollector<Message> outputValidationQueue)
        {
            _logger.LogInformation($"C# ServiceBus queue trigger function recieved message Id: {rawMessage.MessageId}");                        

            ProfilePublisherRequest profilePublisherRequest = ServiceBusMessageProcessor.GetMessage(rawMessage);

            var validRequest = _validator.ValidateRequest(profilePublisherRequest);

            if (!validRequest)
            {                
                _logger.LogError($"Not a valid request to be posted");
                
                return;
            }

            try
            {
                await _OneWorldProfileSyncProcessor.ProcessSyncRequestAsync(profilePublisherRequest, rawMessage.Clone(), outputValidationQueue);

                _logger.LogInformation($"Process profile successfully.");

            }
            catch (UnauthorizedException unauthorizeException)
            {
                _logger.LogError(unauthorizeException, "Authentication Failed");
            }
            catch (InvalidSessionException invalidSessionException)
            {
                _logger.LogError(invalidSessionException, "Invalid Session returned from Authentication Request");
            }
            catch (ProfileUpsertException profileUpsertException)
            {
                _logger.LogInformation($"{profileUpsertException.Message}");
                _logger.LogError(profileUpsertException, $"Update/Insert profile failed for message: {JsonConvert.SerializeObject(profilePublisherRequest)}");
                ServiceBusMessageProcessor.ReQueueMessage(rawMessage, outputSbQueue);
            }
            catch (ProfileDeleteException profileDeleteException)
            {
                _logger.LogError(profileDeleteException, $"Delete profile failed for messag: {JsonConvert.SerializeObject(profilePublisherRequest)}");
                ServiceBusMessageProcessor.ReQueueMessage(rawMessage, outputSbQueue);
            }
            catch (AmadeusRequestException amadeusRequestException)
            {
                _logger.LogError(amadeusRequestException, $"Request failed with status code {amadeusRequestException.HttpStatusCode} while proccessing message: {JsonConvert.SerializeObject(profilePublisherRequest)}");
                ServiceBusMessageProcessor.ReQueueMessage(rawMessage, outputSbQueue);
            }

            _logger.LogInformation("C# ServiceBus queue trigger function OneWorldProfileSync processed message");
        }
    }
}
