using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers
{
    public interface IValidator
    {
        bool ValidateRequest(ProfilePublisherRequest profilePublisherRequest);
    }
}