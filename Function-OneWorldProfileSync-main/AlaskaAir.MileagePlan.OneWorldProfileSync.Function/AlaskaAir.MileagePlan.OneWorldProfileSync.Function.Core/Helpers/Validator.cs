using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using Microsoft.Extensions.Logging;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers
{
    public class Validator : IValidator
    {
        private readonly ILogger<Validator> _logger;

        public Validator(ILogger<Validator> logger)
        {
            _logger = logger;
        }

        public bool ValidateRequest(ProfilePublisherRequest profilePublisherRequest)
        {
            _logger.LogInformation("Validating request");

            if (string.IsNullOrEmpty(profilePublisherRequest?.FirstName))
            {
                _logger.LogError($"FirstName cannot be null");
            }
            else if (string.IsNullOrEmpty(profilePublisherRequest?.LastName))
            {
                _logger.LogError($"LastName cannot be null");
            }
            else if (string.IsNullOrEmpty(profilePublisherRequest?.ModificationNumber))
            {
                _logger.LogError($"ModificationNumber cannot be null");
            }
            else if (string.IsNullOrEmpty(profilePublisherRequest?.MileagePlanNumber))
            {
                _logger.LogError($"MileagePlanNumber cannot be null");
            }
            else
            {
                _logger.LogInformation("Request is valid");

                return true;
            }

            return false;
        }
    }
}