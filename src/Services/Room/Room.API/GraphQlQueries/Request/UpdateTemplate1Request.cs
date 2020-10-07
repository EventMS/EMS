
using System.ComponentModel.DataAnnotations;

namespace EMS.Room_Services.API.Controllers.Request
{
    public class UpdateRoomRequest
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
    }
}
