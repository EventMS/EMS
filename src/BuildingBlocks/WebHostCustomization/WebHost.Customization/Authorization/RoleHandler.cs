using System;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog;

namespace EMS.TemplateWebHost.Customization.StartUp
{
    public class RoleHandler : AuthorizationHandler<RoleRequirement, Guid>
    {
        private readonly PermissionService _permissionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoleHandler(PermissionService permissionService, IHttpContextAccessor httpContextAccessor)
        {
            _permissionService = permissionService;
            _httpContextAccessor = httpContextAccessor;
        }


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RoleRequirement requirement,
            Guid id)
        {
            _httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationToken);
            if (authorizationToken.Count != 0 && await _permissionService.GetPermissions(id, requirement.Role, authorizationToken[0]) == requirement.Role)
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