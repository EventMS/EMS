using System;
using Microsoft.AspNetCore.Identity;

namespace EMS.Identity_Services.API.Context.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
