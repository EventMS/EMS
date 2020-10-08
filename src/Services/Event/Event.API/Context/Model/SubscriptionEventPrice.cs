using System;

namespace EMS.Event_Services.API.Context.Model
{
    public class SubscriptionEventPrice
    {
        public Guid SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public int Price { get; set; }
    }
}