using System.ComponentModel.DataAnnotations;

namespace EMS.Payment_Services.API.Controllers.Request
{
    public class CreatePaymentRequest
    {
        [MaxLength(25)]
        [Required]
        public string Name { get; set; }
    }
}
