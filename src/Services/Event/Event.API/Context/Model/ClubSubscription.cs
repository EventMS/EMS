using System;
using System.Collections.Generic;

namespace EMS.Event_Services.API.Context.Model
{
    public class ClubSubscription
    {
        public Guid ClubId { get; set; }

        public Guid ClubSubscriptionId { get; set; }
        public List<EventPrice> EventPrices { get; set; }
    }
}