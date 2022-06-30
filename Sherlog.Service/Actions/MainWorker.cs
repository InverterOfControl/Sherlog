using Sherlog.Shared.Helper;
using Sherlog.Shared.Models;

namespace Sherlog.Service.Actions
{
  public class MainWorker : IMainWorker
  {
    private readonly ILogger<MainWorker> _logger;
    private readonly IList<ServiceConfiguration> _serviceConfigurations;
    private readonly IDeleter _deleter;

    public MainWorker(ILogger<MainWorker> logger, IList<ServiceConfiguration> serviceConfigurations, IDeleter deleter)
    {
      _logger = logger;
      _serviceConfigurations = serviceConfigurations;
      _deleter = deleter;
    }

    public void DoWork(CancellationToken cancellationToken)
    {
      _logger.LogDebug("Execute mainloop");

      foreach (var service in _serviceConfigurations)
      {
        _logger.LogInformation("Looking for logs for {ServiceName}", service.ServiceName);

        IEnumerable<string> logs;
        try
        {
          logs = Crawler.GetAllFiles(service.LogPath);
        }
        catch (UnauthorizedAccessException ex)
        {
          _logger.LogError(ex, "Error while accessing Path {Path}. Make sure service runs with correct permissions.", service.LogPath);
          continue;
        }
        catch (DirectoryNotFoundException ex)
        {
          _logger.LogError(ex, "Could not find Path {Path}. Double-check your config for {Service}!", service.LogPath, service.ServiceName);
          continue;
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "Something did not work :(");
          continue;
        }

        List<LogProcessModel> logsToProcess = new List<LogProcessModel>();

        foreach (string log in logs)
        {
          var date = Parser.TryExtractDateFromFilename(log);

          if (date == null) continue;

          if ((DateTime.UtcNow - date.Value).Days < service.DaysToKeepUnprocessed)
          {
            continue;
          }

          logsToProcess.Add(new LogProcessModel(log, date.Value));
        }

        var logResults = Grouper.GroupLogs(logsToProcess);

        _logger.LogInformation("Processing {Count} logs", logResults.Count());

        foreach (var logresult in logResults)
        {
          if (service.DoBackups)
          {
            string newFileName = $"{service.BackupPath}\\{logresult.LogTypeName}.{logresult.Timerange}.zip";

            Compressor.Compress(logresult.Filepaths.ToArray(), newFileName);
          }

          _deleter.Delete(logresult.Filepaths.ToArray());
        }
      }
    }
  }
}
