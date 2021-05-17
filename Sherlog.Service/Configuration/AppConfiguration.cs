using System;
using System.Collections.Generic;
using System.Text;

namespace Sherlog.Service.Configuration
{
    public class AppConfiguration
    {
        public int MinutesBetweenChecks { get; set; }

        public string ListenAddress { get; set; }

        public int ListenPort { get; set; }
    }
}
