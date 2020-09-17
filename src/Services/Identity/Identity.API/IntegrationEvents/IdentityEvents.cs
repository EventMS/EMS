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

    public class UserUpdatedIntegrationEvent : IntegrationEvent // All events should inherit from Integration event
    {
        public string UserId { get; set; }
        public string Name { get; set; }

        public UserUpdatedIntegrationEvent(string userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}