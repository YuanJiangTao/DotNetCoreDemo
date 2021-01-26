using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace SerilogDemo
{
    public class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            logger.Information("Hello, world!");

            Log.Logger = logger;
            Log.Information("Test Log");

            var collection = new ServiceCollection();
            ConfigureServices(collection);
            Log.Warning("Test Seq");
            var serviceProvider = collection.BuildServiceProvider();
            serviceProvider.GetService<TestA>().Test();


            Console.ReadLine();
        }

        static void TestSeq()
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Seq("http://localhost:5341/")
               //.WriteTo.File("logs\\myapp.txt", rollingInterval: RollingInterval.Day)
               .CreateLogger();
            var duixiang = new
            {
                name = "这是个对象",
                people = 666
            };
            int i = 0;
            try
            {
                double s = 1 / i;
            }
            catch (Exception ex)
            {
                Log.Verbose(ex, "Something went wrong");
                //throw;
            }
            Log.Debug("Hello, world!");
            Log.Information("Hello, world!{@Ca}", duixiang);
            Log.Debug("Hello, world!");
            Log.Error("Hello, world!");
            Log.Warning("Hello, world!");
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggBuilder =>
            {
                loggBuilder.AddSerilog(Log.Logger);
            });
            services.AddSingleton<TestA>();
        }
    }

    public class TestA
    {
        private readonly ILogger<TestA> _logger;

        public TestA(ILogger<TestA> logger)
        {
            _logger = logger;
        }
        public void Test()
        {
            _logger.LogInformation($"Test From DI Logger");
        }

    }
}
