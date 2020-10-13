using System;

namespace EMS.EventParticipant_Services.API.Context.Model
{
    public class EventPrice
    {
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public float? Price { get; set; }
        public Guid? SubscriptionId { get; set; }
    }
}