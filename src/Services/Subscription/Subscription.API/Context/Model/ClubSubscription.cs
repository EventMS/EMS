using System;

namespace Subscription.API.Context
{
    public class ClubSubscription
    {
        public Guid SubscriptionId { get; set; }
        public Club Club { get; set; }
        public Guid ClubId { get; set; }
        public string Name { get; set; }

        public int Price { get; set; }

        public ClubSubscription() { }
    }
}
