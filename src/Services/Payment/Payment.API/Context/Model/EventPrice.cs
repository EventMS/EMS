using System;

namespace EMS.Payment_Services.API.Context.Model
{
    public class EventPrice
    {
        public Guid EventId { get; set; }
        public Guid ClubSubscriptionId { get; set; }
        public float Price { get; set; }
        public Event Event { get; set; }
    }
}