using System;
using System.Collections.Generic;

namespace EMS.Event_Services.API.Context.Model
{
    public class Room
    {
        public Guid ClubId { get; set; }

        public Guid RoomId { get; set; }
    }

    public class RoomEvent
    {
        public Guid ClubId { get; set; }
        public Club Club { get; set; }

        public Guid RoomId { get; set; }
        public Room Room { get; set; }
    }

    public class Club
    {
        public Guid ClubId { get; set; }

        public List<Instructor> Instructors { get; set; }

        public List<Event> Events { get; set; }
    }
}