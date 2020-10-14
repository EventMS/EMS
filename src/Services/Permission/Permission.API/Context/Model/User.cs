using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace EMS.Permission_Services.API.Context.Model
{
    public class User
    {
        public Guid UserId { get; set; }

        public ICollection<Role> Roles { get; set; }

        public User() { }
    }
}
