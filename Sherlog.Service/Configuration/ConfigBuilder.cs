using Sherlog.Shared.Models;

namespace Sherlog.Service.Configuration
{
  public class ConfigBuilder
  {
    public static IEnumerable<ServiceConfiguration> BuildServiceConfigs()
    {
      var fileNames = Directory
          .GetFiles(AppDomain.CurrentDomain.BaseDirectory + "ServiceConfigurations", "*.json")
          .Where(file => !file.Contains("template"))
          .ToList();

      if (fileNames.Count == 0)
      {
        yield break;
      }

      foreach (string filename in fileNames)
      {
        yield return GetConfig<ServiceConfiguration>(filename);
      }
    }

    internal static T GetConfig<T>(string jsonFile)
    {
      var configRoot = new ConfigurationBuilder()
          .AddJsonFile(jsonFile)
          .Build();

      return configRoot.Get<T>();
    }
  }
}
