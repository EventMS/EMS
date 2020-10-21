using System;

namespace EMS.PaymentWebhook_Services.API.GraphQlQueries
{
    public class SubCheatRequest
    {
        public Guid UserId { get; set; }
        public Guid ClubSubscriptionId { get; set; }
    }
}