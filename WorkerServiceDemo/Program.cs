using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace WorkerServiceDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        private static string GetBasePath()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;
            if (isDevelopment)
            {
                return Directory.GetCurrentDirectory();
            }
            using var processModule = Process.GetCurrentProcess().MainModule;
            return Path.GetDirectoryName(processModule?.FileName);
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, configuration) =>
            {
                configuration.SetBasePath(GetBasePath())
                .SetFileLoadExceptionHandler(c =>
                {
                    Log.Warning(c.Exception, "FileLoadException");
                })
                .AddJsonFile("appsettings.json").
                AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            })
            .UseWindowsService()
            .UseSerilog((hostBuilderContext, loggingBuilder) =>
            {
                loggingBuilder.ReadFrom.Configuration(hostBuilderContext.Configuration);
            })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddOptions()
                    .Configure<Setting>(hostContext.Configuration.GetSection("Setting"));
                });
    }

    public class Setting
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return $"Name:{Name};Age:{Age}";
        }
    }
}
