using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;




namespace EMS.Club_Service.API.Context.Model
{
  
    

    public class Club : IValidatableObject
    {
        public Guid ClubId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Address { get; set; }
        public Guid AdminId { get; set; } 
        public HashSet<Guid> InstructorIds { get; set; }

        public IEnumerable<ValidationResult> Validate([Service] ValidationContext validationContext)
        {
            if (Name.Length > 25)
            {
                yield return new ValidationResult(
                    "Name must be shorter than 25 charactors",
                    new[] { nameof(Name)});
            }
            if (PhoneNumber.Length != 8)
            {
                yield return new ValidationResult(
                    "PhoneNumber must be exactly than 8 charactors",
                    new[] { nameof(PhoneNumber) });
            }
            if (Address.Length > 50)
            {
                yield return new ValidationResult(
                    "Address must be shorter than 50 charactors",
                    new[] { nameof(Address) });
            }
            if (Description.Length > 500)
            {
                yield return new ValidationResult(
                    "Description must be shorter than 500 charactors",
                    new[] { nameof(Description) });
            }
        }
    }
}
