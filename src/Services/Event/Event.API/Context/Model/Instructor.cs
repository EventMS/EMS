using System;
using System.Collections.Generic;

namespace EMS.Event_Services.API.Context.Model
{
    public class Instructor
    {
        public Guid InstructorId { get; set; }

        public Guid ClubId { get; set; }

        public List<InstructorForEvent> InstructorForEvents { get; set; }
        public Instructor() { }
    }
}