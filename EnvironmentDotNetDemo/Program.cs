using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
namespace EnvironmentDotNetDemo
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var env = hostContext.HostingEnvironment;
                    if (env.IsDevelopment())
                    {
                        Console.WriteLine($"Development");
                    }
                    Console.WriteLine($"Hosting Environment:  + {env.EnvironmentName}");
                });
    }
}
