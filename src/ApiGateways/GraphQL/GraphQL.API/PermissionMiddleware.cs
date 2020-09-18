using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace GraphQL.API
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, PermissionService _permissionClient)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                try
                {
                    var bodyStr = "";
                    var req = context.Request;
                    req.EnableBuffering();
                    using (StreamReader reader
                        = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
                    {
                        bodyStr = await reader.ReadToEndAsync();
                    }
                    req.Body.Position = 0;
                    Log.Information(bodyStr);
                   // var token = await _permissionClient.GetPermissions(bodyStr);
                    //context.Request.Headers.Remove("Authorization");
                   // context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
                catch (Exception e)
                {
                    Log.Information(e.Message);
                    Log.Information("Permission request failed");
                }
            }
            await _next(context);
        }
    }
}
