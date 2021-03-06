﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using EMS.TemplateWebHost.Customization.StartUp;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace EMS.GraphQL.API
{
    /// <summary>
    /// Middleware that intercept request, and attaches new Bearer token with permission from Permission Service.
    /// </summary>
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;
        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, PermissionService permissionClient)
        {
            if (context.Request.Headers.ContainsKey("X-Apollo-Tracing"))
            {
                //Playground introspection query support
                await _next(context);
            }
            else if (context.Request.Headers.ContainsKey("Authorization"))
            {
                try
                {
                    var token = await permissionClient.GetFatToken();
                    context.Request.Headers.Remove("Authorization");
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                    await _next(context);
                }
                catch (Exception e)
                {
                    Log.Information("User did not exist");
                    Log.Information(e.Message);
                    await context.Response.WriteAsync("{\"errors\": [{\"message\" : \"User did not exist\"}], \"data\" : null}");
                    //Short circuit request because of invalid token. 
                }
            }   
            else
            {
                await _next(context); //Forwardning just in case. 
            }
        }
    }
}
