using System.ComponentModel.DataAnnotations;

namespace EMS.PaymentWebhook_Services.API.Controllers.Request
{
    public class CreatePaymentWebhookRequest
    {
        [MaxLength(25)]
        [Required]
        public string Name { get; set; }
    }
}
