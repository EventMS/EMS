using System;

namespace EMS.PaymentWebhook_Services.API.GraphQlQueries
{
    public class EventCheatRequest
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
    }
}