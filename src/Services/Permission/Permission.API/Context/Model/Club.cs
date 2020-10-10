using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace EMS.Permission_Services.API.Context.Model
{
    public class Club : IValidatableObject
    {
        public Guid ClubId { get; set; }

        public ICollection<Role> Users { get; set; }


        public Club() { }
        public IEnumerable<ValidationResult> Validate([Service] ValidationContext validationContext)
        {
            yield break;
        }
    }
}