namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Constants
{
    public static class EventTypes
    {
        public static readonly string TierUpgradeEventType = "TIER_UPGRADE";
        public static readonly string TierDowngradeEventType = "TIER_DOWNGRADE";
        public static readonly string TierDowngradeRegularEventType = "TIER_DOWNGRADE_REGULAR";
        public static readonly string MemberUpdateEventType = "MEMBER_UPDATE";
        public static readonly string MemberDeleteEventType = "MEMBER_DELETE";
        public static readonly string BatchUploadEventType = "BATCH_UPLOAD";
        public static readonly string BatchRefreshEventType = "BATCH_REFRESH";
    }
}