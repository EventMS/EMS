using System;
using System.Collections.Generic;

namespace EMS.Event_Services.API.Context.Model
{
    public class Room
    {
        public Guid ClubId { get; set; }
        public Guid RoomId { get; set; }
        public List<RoomEvent> Locations { get; set; }
    }
}