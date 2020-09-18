using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TemplateWebHost.Customization;
using TemplateWebHost.Customization.Settings;
using TemplateWebHost.Customization.StartUp;

namespace GraphQL.API
{
    public class Program
    {
        public static string AppName = "GraphQL.API";
        public static int Main(string[] args)
        {
            return new BaseProgramHelper<Startup>(AppName).Run(args);
        }
    }
}
