using System;

namespace Sherlog.Shared.Models
{
    public class LogProcessModel
    {
        public string LogName { get; set; }

        public DateTime LogDate { get; set; }

        public LogProcessModel(string name, DateTime date)
        {
            LogName = name;
            LogDate = date;
        }
    }
}
