using System;

namespace EMS.Event_Services.API.Context.Model
{
    public class RoomEvent
    {
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public Guid RoomId { get; set; }
        public Room Room { get; set; }
    }
}