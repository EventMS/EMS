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
        [HttpGet]
        public async Task<string> GetFatToken()
        {
            var userId = new Guid(User.FindFirstValue("id"));
            var userPermissions = await _permissionContext.Roles
                .Where(user => user.UserId == userId)
                .ToListAsync();
            return _jwtService.GenerateJwtToken(userId, userPermissions);
        }

        [Authorize]
        [HttpGet]
        [Route("{clubId}/{role}")]
        public async Task<string> GetPermission(Guid clubId, string role)
        {
            var userId = new Guid(User.FindFirstValue("id"));
            var userPermissions = await _permissionContext.Roles
                .Where(user => user.UserId == userId &&
                               user.ClubId == clubId &&
                               user.UserRole == role)
                .FirstOrDefaultAsync();

            Log.Information("User with Id {userId} has permission {role}", userId, userPermissions?.UserRole);
            return userPermissions == null ? "" : role;
        }
    }
}
