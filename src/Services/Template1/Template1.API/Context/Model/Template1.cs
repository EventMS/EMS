using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace Template1.API.Context.Model
{
    public class Template1 : IValidatableObject
    {
        public Guid Id { get; set; }

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
