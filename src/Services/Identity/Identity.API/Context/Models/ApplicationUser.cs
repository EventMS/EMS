using Microsoft.AspNetCore.Identity;

namespace Identity.API.Context.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

    }
}
