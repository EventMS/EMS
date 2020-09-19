using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace Permission.API.Context.Model
{
    public class UserPermission : IValidatableObject
    {
        public Guid UserId { get; set; }

        public ICollection<UserAdministratorPermission> ClubAdminIn { get; set; }

        public UserPermission() { }
        public IEnumerable<ValidationResult> Validate([Service] ValidationContext validationContext)
        {
            yield break;
        }
    }
}
