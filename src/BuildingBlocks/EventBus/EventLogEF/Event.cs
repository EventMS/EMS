using System;
using Newtonsoft.Json;

namespace EMS.BuildingBlocks.EventLogEF
{
    /// <summary>
    /// Base class for all events -
    /// Based on: https://github.com/dotnet-architecture/eShopOnContainers
    /// </summary>
    public class Event
    {
        public Event()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public Event(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public DateTime CreationDate { get; private set; }
    }
}
