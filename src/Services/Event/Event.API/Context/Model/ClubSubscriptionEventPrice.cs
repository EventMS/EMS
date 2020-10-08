using System;

namespace EMS.Event_Services.API.Context.Model
{
    public class ClubSubscriptionEventPrice
    {
        public Guid SubscriptionId { get; set; }
        public ClubSubscription ClubSubscription { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public int Price { get; set; }
    }
}