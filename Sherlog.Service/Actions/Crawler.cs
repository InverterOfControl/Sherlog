using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherlog.Service.Actions
{
  public class Crawler
  {
    public static IEnumerable<string> GetAllFiles(string root)
    {
      Stack<string> pending = new Stack<string>();
      pending.Push(root);
      while (pending.Count != 0)
      {
        var path = pending.Pop();
        string[] next = null;
        try
        {
          next = Directory.GetFiles(path);
        }
        catch { }
        if (next != null && next.Length != 0)
          foreach (var file in next) yield return file;
        try
        {
          next = Directory.GetDirectories(path);
          foreach (var subdir in next) pending.Push(subdir);
        }
        catch { }
      }
    }
  }
}
