using LaptopRequisition.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LaptopRequisition.WebAPI.Services
{
    public class RecycleBinCleanupService : BackgroundService
    {
        private readonly ILogger<RecycleBinCleanupService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromHours(24);

        public RecycleBinCleanupService(ILogger<RecycleBinCleanupService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Recycle Bin Cleanup Service running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Recycle Bin Cleanup Service performing a cleanup operation.");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var recycleBinService = scope.ServiceProvider.GetRequiredService<IRecycleBinService>();
                    try
                    {
                        await recycleBinService.CleanUpRecycleBinAsync();
                        _logger.LogInformation("Recycle Bin Cleanup operation completed successfully.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred while cleaning up recycle bin.");
                    }
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("Recycle Bin Cleanup Service stopping.");
        }
    }
}