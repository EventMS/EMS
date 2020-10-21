using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace EMS.PaymentWebhook_Services.API.Context.Model
{
    public class PaymentWebhook : IValidatableObject
    {
        public Guid PaymentWebhookId { get; set; }

        public string Name { get; set; }

        public PaymentWebhook() { }
        public IEnumerable<ValidationResult> Validate([Service] ValidationContext validationContext)
        {
            if (Name.Length >3)
            {
                yield return new ValidationResult(
                    "Name must be longer than three charactors",
                    new[] { nameof(Name)});
            }
        }
    }
}
