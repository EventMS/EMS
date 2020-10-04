using System;
using System.Collections.Generic;

namespace EMS.Subscription_Services.API.Context
{
    public class Club
    {
        public Guid ClubId { get; set; }

        public List<ClubSubscription> Subscriptions { get; set; }
    }
}