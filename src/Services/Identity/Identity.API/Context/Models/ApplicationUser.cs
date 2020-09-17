using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.eShopOnContainers.Services.Identity.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

    }
}
