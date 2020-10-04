
using System.ComponentModel.DataAnnotations;

namespace EMS.Template1_Services.API.Controllers.Request
{
    public class UpdateTemplate1Request
    {
        [MaxLength(25)]
        [Required]
        public string Name { get; set; }
    }
}
