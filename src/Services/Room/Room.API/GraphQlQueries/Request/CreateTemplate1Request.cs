using System;
using System.ComponentModel.DataAnnotations;
using EMS.TemplateWebHost.Customization.Attributes;

namespace EMS.Room_Services.API.Controllers.Request
{
    public class CreateRoomRequest
    {
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        [NotEmpty]
        public Guid ClubId { get; set; }
    }
}
