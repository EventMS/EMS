using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMS.Event_Services.API.Context.Model
{
    public class ClubSubscriptionEventPrice : IValidatableObject
    {
        public Guid SubscriptionId { get; set; }
        public ClubSubscription ClubSubscription { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public float Price { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Price < 0)
            {
                yield return new ValidationResult("Price must be zero or above", new []{nameof(Price)});
            }
        }
    }
}