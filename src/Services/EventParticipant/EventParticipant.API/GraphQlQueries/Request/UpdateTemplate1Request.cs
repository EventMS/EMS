
using System.ComponentModel.DataAnnotations;

namespace EMS.EventParticipant_Services.API.Controllers.Request
{
    public class UpdateEventParticipantRequest
    {
        [MaxLength(25)]
        [Required]
        public string Name { get; set; }
    }
}
