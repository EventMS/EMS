using System;

namespace EMS.Event_Services.API.Context.Model
{
    public class InstructorForEvent
    {
        public Guid InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
    }
}