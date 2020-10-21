using System;

namespace EMS.Subscription_Services.API.Context
{
    public class ClubSubscription
    {
        public Guid ClubSubscriptionId { get; set; }
        public Club Club { get; set; }
        public Guid ClubId { get; set; }
        public string Name { get; set; } 

        public int Price { get; set; }

        public ClubSubscription() { }
    }
}
