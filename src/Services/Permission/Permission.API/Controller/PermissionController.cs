using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMS.Permission_Services.API.Context;
using EMS.Permission_Services.API.Services;
using Serilog;

namespace EMS.Permission_Services.API.Controller
{
    public class ContextInRequest
    {
        public string ClubId { get; set; }
        public string EventId { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly PermissionContext _permissionContext;
        private readonly JwtService _jwtService;

        public PermissionController(PermissionContext permissionContext, JwtService jwtService)
        {
            _permissionContext = permissionContext;
            _jwtService = jwtService;
        }

        [Authorize]
        [HttpPost]
        public async Task<string> GetPermission([FromBody] ContextInRequest context)
        {
            var userId = new Guid(User.FindFirstValue("id"));
            if (_permissionContext.UserPermissions.Find(userId) == null)
            {
                Log.Information("Request made for user with token, but user is no longer in the system. ");
                return "No user configured";
            }

            if (context.ClubId != null)
            {
                var userPermissions = await _permissionContext.UserAdministratorPermission
                    .Where(user => user.UserId == userId)
                    .Where(user => user.ClubId == new Guid(context.ClubId))
                    .FirstOrDefaultAsync();
                return _jwtService.GenerateJwtToken(userId, userPermissions);
            }
            Log.Information("There should be more context here to decode the expected context.. ");
            return "Invalid token";
        }

        [Authorize]
        [HttpGet]
        [Route("{clubId}")]
        public async Task<string> GetPermission(Guid clubId)
        {
            Log.Information("clubId: " + clubId.ToString());
            var userId = new Guid(User.FindFirstValue("id"));
            Log.Information("userId: " + userId.ToString());
            if (_permissionContext.UserPermissions.Find(userId) == null)
            {
                Log.Information("Request made for user with token, but user is no longer in the system. ");
                return "";
            }

            var userPermissions = await _permissionContext.UserAdministratorPermission
                .Where(user => user.UserId == userId)
                .Where(user => user.ClubId == clubId)
                .FirstOrDefaultAsync();
            return userPermissions == null ? "" : "Admin";
        }
    }
}
