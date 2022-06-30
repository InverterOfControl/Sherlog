using Serilog;
using System.IO;

namespace Sherlog.Service.Actions
{
  public class Deleter : IDeleter
  {
    private readonly ILogger<Deleter> _logger;

    public Deleter(ILogger<Deleter> logger)
    {
      _logger = logger;
    }

    public void Delete(string[] files)
    {
      Log.Debug($"Deleting {string.Join(", ", files)}");

      foreach (string file in files)
      {
        try
        {
          File.Delete(file);
        }
        catch (Exception ex) {
          _logger.LogError($"Could not delete file {file}", ex);
        }
      }
    }
  }
}
