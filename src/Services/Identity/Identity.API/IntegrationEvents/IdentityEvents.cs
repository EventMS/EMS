using EMS.BuildingBlocks.EventLogEF;


namespace EMS.Events
{
    public class UserCreatedEvent : Event // All events should inherit from Integration event
    {
        public string StripeCustomerId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
    }

    public class UserUpdatedEvent : Event // All events should inherit from Integration event
    {
        public string UserId { get; set; }
        public string Name { get; set; }

        public UserUpdatedEvent(string userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}