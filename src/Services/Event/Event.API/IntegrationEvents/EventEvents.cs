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

        public EventType EventType { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public List<EventPrice> EventPrices { get; set; }

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

        public List<EventPrice> EventPrices { get; set; }

        public List<RoomEvent> Locations { get; set; }

        public List<InstructorForEvent> InstructorForEvents { get; set; }

    }

    public class EventDeletedEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
    }

    public class EventCreationFailedEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
        public string Reason { get; set; }
    }

    public class InstructorAddedEvent : Event
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
        public Guid ClubSubscriptionId { get; set; }
        public Guid ClubId { get; set; }
        public Guid? ReferenceId { get; set; }
    }

    public class EventPrice
    {
        public Guid ClubSubscriptionId { get; set; }
        public float Price { get; set; }
    }

    public class TimeslotReservationFailedEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
        public string Reason { get; set; }
    }

    public class TimeslotReservedEvent : Event // All events should inherit from Integration event
    {
        public Guid EventId { get; set; }
    }

    public class VerifyAvailableTimeslotEvent : Event // All events should inherit from Integration event
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid EventId { get; set; }
        public List<Guid> RoomIds { get; set; }
    }

    public class VerifyChangedTimeslotEvent : Event // All events should inherit from Integration event
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid EventId { get; set; }
        public List<Guid> RoomIds { get; set; }
    }

    public class RoomCreatedEvent : Event // All events should inherit from Integration event
    {
        public Guid ClubId { get; set; }
        public Guid RoomId { get; set; }
        public string Name { get; set; }
    }
}

