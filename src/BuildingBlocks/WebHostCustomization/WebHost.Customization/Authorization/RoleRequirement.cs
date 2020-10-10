using Microsoft.AspNetCore.Authorization;

namespace EMS.TemplateWebHost.Customization.StartUp
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; }

        public RoleRequirement(string role)
        {
            Role = role;
        }
    }
}