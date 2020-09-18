using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace Permission.API.Context.Model
{
    public class Permission : IValidatableObject
    {
        public Guid Id { get; set; }

       // public string UserId { get; set; }

       // public Guid ClubId { get; set; }

        public Permission() { }
        public IEnumerable<ValidationResult> Validate([Service] ValidationContext validationContext)
        {
            yield break;
        }
    }
}
