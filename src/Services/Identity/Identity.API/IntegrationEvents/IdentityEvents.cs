using EMS.BuildingBlocks.EventLogEF;


namespace EMS.Events
{
    public class UserCreatedEvent : Event // All events should inherit from Integration event
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class UserUpdatedEvent : Event // All events should inherit from Integration event
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public UserUpdatedEvent(string userId, string name, string phoneNumber)
        {
            UserId = userId;
            Name = name;
            PhoneNumber = phoneNumber;
        }
    }
}