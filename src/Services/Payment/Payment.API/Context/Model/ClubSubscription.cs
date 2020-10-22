using System;

namespace EMS.Payment_Services.API.Context.Model
{
    public class ClubSubscription
    {
        public Guid ClubSubscriptionId { get; set; }
        public Guid ClubId { get; set; }
        public string StripePriceId { get; set; }
        public string StripeProductId { get; set; }
    }
}