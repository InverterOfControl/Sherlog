using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherlog.Service.Actions
{
    public class Mover
    {
        public static void Move(string filepath, string newpath)
        {
            Log.Debug($"Moving {filepath} to {newpath}");
        }
    }
}
