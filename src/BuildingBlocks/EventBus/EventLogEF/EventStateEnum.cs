namespace EMS.BuildingBlocks.IntegrationEventLogEF
{
    /// <summary>
    /// State that the eventlogentry can be in
    /// Based on: https://github.com/dotnet-architecture/eShopOnContainers
    /// </summary>
    public enum EventStateEnum
    {
        NotPublished = 0,
        InProgress = 1,
        Published = 2,
        PublishedFailed = 3
    }
}
