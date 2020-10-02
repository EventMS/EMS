using System;
using System.Collections.Generic;

namespace Subscription.API.Context
{
    public class Club
    {
        public Guid ClubId { get; set; }

        public List<ClubSubscription> Subscriptions { get; set; }
    }
}