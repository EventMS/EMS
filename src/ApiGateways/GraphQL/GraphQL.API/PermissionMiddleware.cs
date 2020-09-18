using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using Serilog;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
                    var body = await ReadFromBodyOfRequest(context.Request);
                   var content = JsonConvert.DeserializeObject<Content>(body);
                   var token = await _permissionClient.GetPermissions(content);
                    //context.Request.Headers.Remove("Authorization");
                   // context.Request.Headers.Add("Authorization", "Bearer " + token);
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
