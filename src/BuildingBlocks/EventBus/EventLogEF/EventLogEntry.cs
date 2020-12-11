using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EMS.BuildingBlocks.IntegrationEventLogEF;
using Newtonsoft.Json;

namespace EMS.BuildingBlocks.EventLogEF
{
    /// <summary>
    /// Database entry that wraps a event
    /// Based on: https://github.com/dotnet-architecture/eShopOnContainers
    /// </summary>
    public class EventLogEntry
    {
        private EventLogEntry() { }
        public EventLogEntry(Event @event, Guid transactionId)
        {
            EventId = @event.Id;            
            CreationTime = @event.CreationDate;
            EventTypeName = @event.GetType().FullName;
            Content = JsonConvert.SerializeObject(@event);
            State = EventStateEnum.NotPublished;
            TimesSent = 0;
            TransactionId = transactionId.ToString();
        }
        public Guid EventId { get; private set; }
        public string EventTypeName { get; private set; }
        [NotMapped]
        public string EventTypeShortName => EventTypeName.Split('.')?.Last();
        [NotMapped]
        public Event Event { get; private set; }
        [NotMapped]
        public Type Type { get; private set; }
        public EventStateEnum State { get; set; }
        public int TimesSent { get; set; }
        public DateTime CreationTime { get; private set; }
        public string Content { get; private set; }
        public string TransactionId { get; private set; }

        public EventLogEntry DeserializeJsonContent(Type type)
        {
            Type = type;
            Event = JsonConvert.DeserializeObject(Content, type) as Event;
            return this;
        }
    }
}
