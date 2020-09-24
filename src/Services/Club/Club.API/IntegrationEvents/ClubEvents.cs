using System;
using EMS.BuildingBlocks.IntegrationEventLogEF;

namespace EMS.Events
{
    public class UserCreatedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public string UserId { get; set; }
        public string Name { get; set; }
    }

    public class ClubCreatedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Address { get; set; }
        public Guid AdminId { get; set; }
    }

    public class ClubUpdatedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Address { get; set; }
    }

    public class ClubDeletedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
    }
}
