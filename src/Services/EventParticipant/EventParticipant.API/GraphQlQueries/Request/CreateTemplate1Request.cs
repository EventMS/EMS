using System;
using System.ComponentModel.DataAnnotations;

namespace EMS.EventParticipant_Services.API.Controllers.Request
{
    public class SignUpEventRequest
    {
        public Guid EventId { get; set; }
    }
}
