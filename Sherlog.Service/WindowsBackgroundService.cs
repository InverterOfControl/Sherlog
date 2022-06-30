using Microsoft.Extensions.Options;
using Sherlog.Service.Actions;
using Sherlog.Service.Configuration;

namespace Sherlog.Service
{
  public class WindowsBackgroundService : BackgroundService
  {
    private readonly ILogger<WindowsBackgroundService> _logger;
    private readonly IMainWorker _worker;
    private readonly IOptions<AppConfiguration> _options;

    public WindowsBackgroundService(ILogger<WindowsBackgroundService> logger, IMainWorker worker, IOptions<AppConfiguration> options)
    {
      _logger = logger;
      _worker = worker;
      _options = options;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        _worker.DoWork(stoppingToken);
        
        await Task.Delay(_options.Value.MinutesBetweenChecks * 60000, stoppingToken);
      }
    }
  }
}