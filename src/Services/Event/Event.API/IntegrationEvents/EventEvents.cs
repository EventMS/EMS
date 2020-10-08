using System;
using System.Collections.Generic;
using EMS.Event_Services.API.Context.Model;
using Event = EMS.BuildingBlocks.EventLogEF.Event;

namespace EMS.Events
{
    public class EventCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }

        public Guid ClubId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public List<SubscriptionEventPrice> SubscriptionEventPrices { get; set; }

        public List<RoomEvent> Locations { get; set; }

        public List<InstructorForEvent> InstructorForEvents { get; set; }

    }

    public class EventUpdatedEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }

        public Guid ClubId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public List<SubscriptionEventPrice> SubscriptionEventPrices { get; set; }

        public List<RoomEvent> Locations { get; set; }

        public List<InstructorForEvent> InstructorForEvents { get; set; }

    }

    public class EventDeletedEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
        public Guid ClubId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
        public string Reason { get; set; }
    }

    public class EventCreationFailed : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
    }

    public class InstructorAddedEvent : Event
    {
        public Guid ClubId { get; set; }
        public Guid UserId { get; set; }
    }

    public class InstructorDeletedEvent : Event
    {
        public Guid ClubId { get; set; }
        public Guid UserId { get; set; }
    }
    public class ClubCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
    }

    public class ClubSubscriptionCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid SubscriptionId { get; set; }
        public Guid ClubId { get; set; }
    }

    public class TimeslotReservationFailed : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
        public Guid RoomId { get; set; }
        public string Reason { get; set; }
    }

    public class TimeslotReserved : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
        public Guid RoomId { get; set; }
    }

    public class VerifyAvailableTimeslotEvent : Event // All events should inherit from Integration event
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid EventId { get; set; }
        public Guid RoomId { get; set; }
    }

    public class RoomCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public Guid RoomId { get; set; }
        public string Name { get; set; }
    }

    public class RoomUpdatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public Guid RoomId { get; set; }
        public string Name { get; set; }
    }

    public class RoomDeletedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public Guid RoomId { get; set; }

        public string Name { get; set; }
    }
}

