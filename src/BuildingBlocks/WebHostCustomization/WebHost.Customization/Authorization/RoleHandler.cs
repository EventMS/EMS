using System;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace EMS.TemplateWebHost.Customization.StartUp
{
    public class RoleHandler : AuthorizationHandler<RoleRequirement, Guid>
    {
        private readonly PermissionService _permissionService;

        public RoleHandler(PermissionService permissionService)
        {
            _permissionService = permissionService;
        }


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RoleRequirement requirement,
            Guid id)
        {
            if (await _permissionService.GetPermissions(id) == requirement.Role)
            {
                context.Succeed(requirement);
            }
            else
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("You do not have permission for this action")
                        .SetCode("USER_NOT_AUTH")
                        .Build());
            }
        }
    }
}