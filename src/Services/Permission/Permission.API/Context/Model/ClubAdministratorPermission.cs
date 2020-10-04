using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace EMS.Permission_Services.API.Context.Model
{
    public class ClubAdministratorPermission : IValidatableObject
    {
        public Guid ClubId { get; set; }

        public ICollection<UserAdministratorPermission> Users { get; set; }


        public ClubAdministratorPermission() { }
        public IEnumerable<ValidationResult> Validate([Service] ValidationContext validationContext)
        {
            yield break;
        }
    }
}