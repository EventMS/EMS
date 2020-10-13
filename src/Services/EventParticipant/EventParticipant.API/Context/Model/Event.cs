using System;
using System.Collections.Generic;

namespace EMS.EventParticipant_Services.API.Context.Model
{
    public class Event
    {
        public Guid EventId { get; set; }
        public Guid ClubId { get; set; }
        public EventType? EventType { get; set; }
        public bool? IsFree { get; set; }
        public List<EventParticipant> EventParticipants { get; set; }

        public List<EventPrice> EventPrices { get; set; }
    }

    public enum EventType
    {
        Public, Private
    }
}