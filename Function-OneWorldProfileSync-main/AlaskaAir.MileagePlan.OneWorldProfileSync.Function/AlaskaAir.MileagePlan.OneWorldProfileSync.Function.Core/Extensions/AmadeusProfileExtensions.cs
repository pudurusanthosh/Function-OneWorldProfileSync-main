using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Extensions
{
    public static class AmadeusProfileExtensions
    {
        public static ValidationResult ValidateUpdate(this AMA_ProfileReadRS amadeusProfile, ProfilePublisherRequest request)
        {
            var result = new ValidationResult();
            var customer = amadeusProfile.Profiles?.ProfileInfo?.Profile?.Customer;

            if (customer == default) 
            {
                result.ValidationMessages.Add("No profile returned.");
                return result;
            }

            var oneWorldTeir = customer?.CustLoyalty?.LoyalLevelDescription;
            var lastName = customer.PersonName?.Surname;
            var firstName = customer.PersonName?.GivenName;

            if (oneWorldTeir != request.OneWorldTier)
            {
                result.ValidationMessages.Add($"One World Teir did not update. Expected '{request.OneWorldTier}' was '{oneWorldTeir}'.");
            }

            if (lastName != request.LastName)
            {
                result.ValidationMessages.Add($"Lastname did not update. Expected '{request.LastName}' was '{lastName}'.");
            }

            if (firstName != request.FirstName)
            {
                result.ValidationMessages.Add($"Firstname did not update. Expected '{request.FirstName}' was '{firstName}'.");
            }

            return result;
        }
    }

    public class ValidationResult
    {
        public bool IsValid { 
            get 
            { 
                return ValidationMessages.Any(); 
            } 
        }

        public List<string> ValidationMessages { get; set; }
    }
}
