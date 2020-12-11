using Microsoft.AspNetCore.Authorization;

namespace EMS.TemplateWebHost.Customization.StartUp
{
    /// <summary>
    /// Authorisation requirement as role. 
    /// </summary>
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; }

        public RoleRequirement(string role)
        {
            Role = role;
        }
    }
}