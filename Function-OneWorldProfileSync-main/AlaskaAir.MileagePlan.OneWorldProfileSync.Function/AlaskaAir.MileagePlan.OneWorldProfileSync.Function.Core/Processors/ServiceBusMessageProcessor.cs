using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using System;
using System.Text;
using System.Text.Json;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Processors
{
    public static class ServiceBusMessageProcessor
    {
        public static ProfilePublisherRequest GetMessage(Message rawMessage)
        {
            string message = Encoding.Default.GetString(rawMessage.Body);

            return JsonSerializer.Deserialize<ProfilePublisherRequest>(message);
        }

        public static Message ReQueueMessage(Message rawMessage, ICollector<Message> outputSbQueue)
        {
            var retryMessage = rawMessage.Clone();

            var isRetry = retryMessage.UserProperties.ContainsKey("retryCount");

            if (!isRetry)
            {
                retryMessage.TimeToLive = TimeSpan.FromDays(3);
                retryMessage.UserProperties["retryCount"] = 0;
            }

            retryMessage.UserProperties["retryCount"] = (int)retryMessage.UserProperties["retryCount"] + 1;

            retryMessage.ScheduledEnqueueTimeUtc = DateTime.UtcNow.AddMinutes((int)retryMessage.UserProperties["retryCount"] * 5);

            if (!isRetry || (int)retryMessage.UserProperties["retryCount"] <= 5)
            {
                outputSbQueue.Add(retryMessage);
            }

            return retryMessage;
        }

        public static void SendMessageToTopic(ProfilePublisherRequest profilePublisherRequest, ICollector<string> outputSbTopic)
        {
            string profilePublisherRequestString = Newtonsoft.Json.JsonConvert.SerializeObject(profilePublisherRequest);

            outputSbTopic.Add(profilePublisherRequestString);
        }
    }
}