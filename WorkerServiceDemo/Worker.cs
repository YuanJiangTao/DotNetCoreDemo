using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WorkerServiceDemo
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IOptions<Setting> _options;

        public Worker(ILogger<Worker> logger,IOptions<Setting> options)
        {
            _logger = logger;
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _logger.LogInformation($"Directory:{AppContext.BaseDirectory}");
                _logger.LogInformation($"Options:{_options?.Value.ToString()}");
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
