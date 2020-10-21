using System;

namespace EMS.Subscription_Services.API.GraphQlQueries.Request
{
    public class CreateSubscriptionRequest
    {
        public string PaymentMethodId { get; set; }
        public Guid ClubSubscriptonId { get; set; }
    }
}
