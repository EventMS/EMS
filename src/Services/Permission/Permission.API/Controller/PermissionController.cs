using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Permission.API.Context;
using Permission.API.Services;
using Serilog;

namespace Permission.API.Controller
{
    public class Content
    {
        public string OperationName { get; set; }
        public Request Variables { get; set; }
        public string Query { get; set; }
    }

    public class Request
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
        public async Task<string> GetPermission([FromBody] Content test)
        {
            var userId = new Guid(User.FindFirstValue("id"));

            var userPermissions =  await _permissionContext.UserPermissions
                .Where(user => user.UserId == userId)
                .Include(user => user.ClubAdminIn)
                .SingleAsync();
            return _jwtService.GenerateJwtToken(userPermissions);
        }
    }
}
