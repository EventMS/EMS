using System;
using System.ComponentModel.DataAnnotations;

namespace EMS.Payment_Services.API.Controllers.Request
{
    public class SignUpSubscriptionRequest
    {
        [Required]
        public string PaymentMethodId { get; set; }

        [Required]
        public Guid ClubSubscriptionId { get; set; }
    }
}