using Serilog;
using System.IO;

namespace Sherlog.Service.Actions
{
    public class Deleter
    {
        public static void Delete(string[] files)
        {
            Log.Debug($"Deleting {string.Join(", ", files)}");

            foreach(string file in files)
            {
                File.Delete(file);
            }
        }
    }
}
