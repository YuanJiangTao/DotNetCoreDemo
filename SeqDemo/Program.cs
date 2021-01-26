using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Configuration;
using Serilog.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace SeqDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            CreateHostBuilder(args).Build().Run();
        }
        /*
        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostBuilderContext,services) =>
            {
                services.AddLogging(loggingBuilder =>
                {
                    //此方法默认使用本地地址: 5341端口
                    //加载Seq配置
                    loggingBuilder.AddSeq(hostBuilderContext.Configuration.GetSection("Seq"));

                });
            });
            */
        static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, configuration) =>
            {
                configuration.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").
                AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true).
                AddEnvironmentVariables();
            })
            .UseSerilog((hostBuilderContext, loggingBuilder) =>
        {
            loggingBuilder.ReadFrom.Configuration(hostBuilderContext.Configuration);
        }).
            ConfigureServices(services =>
        {
            services.AddHostedService<TestHostService>();
        });
    }
}
