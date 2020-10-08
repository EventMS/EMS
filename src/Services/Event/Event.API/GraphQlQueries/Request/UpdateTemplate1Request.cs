
using System.ComponentModel.DataAnnotations;

namespace EMS.Event_Services.API.Controllers.Request
{
    public class UpdateEventRequest
    {
        [MaxLength(25)]
        [Required]
        public string Name { get; set; }
    }
}
