using System;
using EMS.BuildingBlocks.IntegrationEventLogEF;

namespace EMS.Events
{
    public class UserCreatedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public UserCreatedIntegrationEvent(string userId, string name)
        {
            UserId = userId;
            Name = name;
        }
        public string UserId { get; set; }
        public string Name { get; set; }
    }

    public class ClubCreatedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public ClubCreatedIntegrationEvent(Guid template1Id, string name)
        {
            ClubId = template1Id;
            Name = name;
        }
        public Guid ClubId { get; set; }
        public string Name { get; set; }
    }

    public class ClubUpdatedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public string Name { get; set; }

        public ClubUpdatedIntegrationEvent(Guid template1Id, string name)
        {
            ClubId = template1Id;
            Name = name;
        }
    }

    public class ClubDeletedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public ClubDeletedIntegrationEvent(Guid template1Id)
        {
            ClubId = template1Id;
        }
    }
}
