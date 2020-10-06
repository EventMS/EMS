using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace EMS.ClubMember_Services.API.Context.Model
{
    public class ClubMember : IValidatableObject
    {
        public Guid UserId { get; set; }
        public Guid ClubId { get; set; }
        public string NameOfSubscription { get; set; }

        public ClubMember() { }
        public IEnumerable<ValidationResult> Validate([Service] ValidationContext validationContext)
        {
            yield break;
        }
    }
}
