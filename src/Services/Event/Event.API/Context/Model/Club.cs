using System;
using System.Collections.Generic;

namespace EMS.Event_Services.API.Context.Model
{
    public class Club
    {
        public Guid ClubId { get; set; }

        public List<Event> Events { get; set; }
    }
}