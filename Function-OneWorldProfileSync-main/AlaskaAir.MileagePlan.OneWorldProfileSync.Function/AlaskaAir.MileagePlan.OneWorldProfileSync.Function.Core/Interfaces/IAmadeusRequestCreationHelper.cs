using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers
{
    public interface IAmadeusRequestCreationHelper
    {
        string Base64Encode(string stringToEncode);
        string GenerateAuthenticationRequest();
        string GenerateProfileAirlineUpsert(Session _session, ProfilePublisherRequest _profilePublisherRequest);
        string GenerateProfileAirlineRefresh(Session _session, ProfilePublisherRequest _profilePublisherRequest);
        string GenerateProfileDelete(Session session, ProfilePublisherRequest profilePublisherRequest);
        string GenerateProfileRetrieveRequest(Session session, ProfilePublisherRequest profilePublisherRequest);
        string GenerateSignoutRequest(Session session);
        OneWorldTierMapping MapToOneWorldTier(string oneWorldTier);
    }
}