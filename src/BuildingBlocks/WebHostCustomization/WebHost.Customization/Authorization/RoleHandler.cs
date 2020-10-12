using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Authorization;
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

        public RoleHandler(PermissionService permissionService)
        {
            _permissionService = permissionService;
        }


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RoleRequirement requirement,
            Guid id)
        {
            var claim = context.User.FindFirst(c => c.Type == "ClubPermissionsClaim");
            if (claim == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("You do not have permission for this action")
                        .SetCode("USER_NOT_AUTH")
                        .Build());
            }

            var clubPermissions = JsonConvert.DeserializeObject<List<ClubPermission>>(claim.Value);
            var club = clubPermissions.FirstOrDefault(club => club.ClubId == id);

            if (club != null && club.UserRole == requirement.Role)
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