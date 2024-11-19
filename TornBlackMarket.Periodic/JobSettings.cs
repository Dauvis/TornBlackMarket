using TornBlackMarket.Periodic.Enums;

namespace TornBlackMarket.Periodic
{
    public record JobSettings(List<JobIdType> JobIdList, string ApiKey);
}
