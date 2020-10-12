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

        public async Task InvokeAsync(HttpContext context, PermissionService _permissionClient)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                //If no cache do request
                try
                {
                    Log.Information("Before: " + context.Request.Headers["Authorization"]);
                    var token = await _permissionClient.GetFatToken();
                    context.Request.Headers.Remove("Authorization");
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                    Log.Information("After: " + context.Request.Headers["Authorization"]);
                }
                catch (Exception e)
                {
                    Log.Information("Permission request failed");
                    Log.Information(e.Message);
                }
            }
            await _next(context);
        }

        private async Task<string> ReadFromBodyOfRequest(HttpRequest request)
        {
            var bodyStr = "";
            request.EnableBuffering();
            using (StreamReader reader
                = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = await reader.ReadToEndAsync();
            }
            request.Body.Position = 0;
            return bodyStr;
        }
    }
}
