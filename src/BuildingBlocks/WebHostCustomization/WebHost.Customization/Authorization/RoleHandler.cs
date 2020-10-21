using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace EMS.TemplateWebHost.Customization.StartUp
{

    public class ClubPermission
    {
        public Guid ClubId { get; set; }
        public string UserRole { get; set; }
        public Guid? SubscriptionId { get; set; }
    }

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
            if (_httpContextAccessor.HttpContext.User == null)
            {
                Log.Information("User does not exist");
                return;
            }
            var claim = _httpContextAccessor.HttpContext.User.FindFirst("ClubPermissionsClaim");
            if (claim.Value == null)
            {
                Log.Information("User have no permissions");
                return;
            }

            var clubPermissions = JsonConvert.DeserializeObject<List<ClubPermission>>(claim.Value);
            var club = clubPermissions.FirstOrDefault(club => club.ClubId == id);

            if (club != null && club.UserRole == requirement.Role)
            {
                Log.Information("User succesfully validated claim: " + requirement.Role + " in " + id);
                context.Succeed(requirement);
            }
            else
            {
                Log.Information("Validation failed, user did not have permissions");
            }
        }
    }
}