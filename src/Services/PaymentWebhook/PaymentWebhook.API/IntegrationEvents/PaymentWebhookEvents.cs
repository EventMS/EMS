using System;
using EMS.BuildingBlocks.EventLogEF;

namespace EMS.Events
{
    public class PaymentWebhookCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid PaymentWebhookId { get; set; }
        public string Name { get; set; }
    }

    public class PaymentWebhookUpdatedEvent : Event // All events should inherit from Integration event
    {
        public Guid PaymentWebhookId { get; set; }
        public string Name { get; set; }
    }

    public class PaymentWebhookDeletedEvent : Event // All events should inherit from Integration event
    {
        public Guid PaymentWebhookId { get; set; }
    }
}
