using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Serilog;
using ILogger = Serilog.ILogger;

namespace EMS.TemplateWebHost.Customization.StartUp
{

    public class BaseProgramHelper<T> where T : class
    {
        private readonly String _appName;
        public BaseProgramHelper(string appName)
        {
            _appName = appName;
        }

        public virtual void MigrateDbContext(IWebHost host)
        {

        }

        public virtual IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
            return builder.Build();
        }


        public static IWebHost CreateHostBuilder(IConfiguration configuration, string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                .CaptureStartupErrors(false)
                .ConfigureKestrel(options =>
                {
                    var port = configuration.GetValue("PORT", 80);
                    options.Listen(IPAddress.Any, port,
                        listenOptions => { listenOptions.Protocols = HttpProtocols.Http1AndHttp2; });
                })
                .UseStartup<T>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSerilog()
                .Build();
        }

        public virtual ILogger CreateSerilogLogger(IConfiguration configuration, string appName)
        {
            var seqServerUrl = configuration["Serilog:SeqServerUrl"];
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", appName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                //.WriteTo.Seq(String.IsNullOrWhiteSpace(seqServerUrl) ? "http://seq" : seqServerUrl)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        public int Run(String[] args)
        {
            var configuration = GetConfiguration();
            Log.Logger = CreateSerilogLogger(configuration, _appName);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", _appName);
                var host = CreateHostBuilder(configuration, args);

                Log.Information("Applying migrations ({ApplicationContext})...", _appName);
                MigrateDbContext(host);

                Log.Information("Starting web host ({ApplicationContext})...", _appName);
                WebHostExtensions.Run(host);

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", _appName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
