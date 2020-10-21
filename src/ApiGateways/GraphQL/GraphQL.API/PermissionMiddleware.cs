using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EMS.TemplateWebHost.Customization.StartUp;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace EMS.GraphQL.API
{


    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, PermissionService permissionClient)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                //If no cache do request
                try
                {
                    var token = await permissionClient.GetFatToken();
                    context.Request.Headers.Remove("Authorization");
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
                catch (Exception e)
                {
                    Log.Information("Permission request failed");
                    Log.Information(e.Message);
                }
            }
            await _next(context);
        }
    }
}
