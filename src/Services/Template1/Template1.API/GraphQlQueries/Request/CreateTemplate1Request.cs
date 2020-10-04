using System.ComponentModel.DataAnnotations;

namespace Template1.API.Controllers.Request
{
    public class CreateTemplate1Request
    {
        [MaxLength(25)]
        [Required]
        public string Name { get; set; }
    }
}