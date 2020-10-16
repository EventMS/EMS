using System;
using System.Collections.Generic;

namespace EMS.EventParticipant_Services.API.Context.Model
{
    public class Event
    {
        public Guid EventId { get; set; }
        public Guid ClubId { get; set; }
        public float? PublicPrice { get; set; }
        public List<EventParticipant> EventParticipants { get; set; }

        public List<EventPrice> EventPrices { get; set; }
    }
}