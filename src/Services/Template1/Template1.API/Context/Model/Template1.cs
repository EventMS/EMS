using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace EMS.Template1_Services.API.Context.Model
{
    public class Template1 : IValidatableObject
    {
        public Guid Template1Id { get; set; }

        public string Name { get; set; }

        public Template1() { }
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
