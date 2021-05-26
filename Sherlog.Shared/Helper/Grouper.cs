using Sherlog.Shared.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sherlog.Shared.Helper
{
    public class Grouper
    {
        private const string serviceInFileNamePattern = "^(.*?)\\.log";

        public static Func<DateTime, int> WeekProjector =
            d => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
             d,
             CalendarWeekRule.FirstFullWeek,
             DayOfWeek.Monday);

        public static Func<string, string> LogNameProjector =
            name =>
            {
                if(Regex.IsMatch(name, serviceInFileNamePattern))
                {
                    return Regex.Match(name, serviceInFileNamePattern).Groups.Values.FirstOrDefault().Value;
                }

                if (name.Contains("u_ex"))
                {
                    return "IIS";
                }

                return "log";
            };

        public static string RemovePath(string fullpath)
        {
            int index = fullpath.LastIndexOf('\\');

            if (index == -1) return fullpath;

            return fullpath.Substring(index + 1);
        }

        public static IEnumerable<GroupedLogModel> GroupLogs(List<LogProcessModel> logsToProcess)
        {
            var groupedLogs = logsToProcess.GroupBy(log =>
            new {
                Name = RemovePath(LogNameProjector(log.LogName)),
                Date = WeekProjector(log.LogDate)
                }
            );

            var logResults = groupedLogs.Select(x =>
                    new GroupedLogModel
                    {
                        LogTypeName = x.Key.Name,
                        Filepaths = x.Select(l => l.LogName),
                        Timerange = $"{x.Min(y => y.LogDate).ToString("yyyyMMdd")}-{x.Max(y => y.LogDate).ToString("yyyyMMdd")}"
                    });

            return logResults;
        }
    }
}
