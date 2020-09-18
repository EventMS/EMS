using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public async Task<string> GetPermission([FromBody] Content test)
        {
            Log.Information("Permissions");
            Log.Information(User.FindFirstValue("id"));
            return test.Query;
        }
    }
}
