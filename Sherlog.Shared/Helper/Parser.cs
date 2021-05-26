using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sherlog.Shared.Helper
{
    public class Parser
    {
        private const string logMatchPattern = "[.]([0-9]{8})+.*\\.txt";

        private const string iisMatchPattern = "u_ex([0-9]{6})";

        public static bool IsLogFile(string filename)
        {
            return Regex.IsMatch(filename, logMatchPattern) || Regex.IsMatch(filename, iisMatchPattern);
        }

        public static DateTime? TryExtractDateFromFilename(string filename, string customRegex = logMatchPattern)
        {
            var match = Regex.Match(filename, customRegex);

            var dateMatch = match.Groups.Values.FirstOrDefault(group => CheckDate(group.Value));

            if(dateMatch != null)
            {
                return ParseDate(dateMatch.Value);
            }

            var iisMatch = Regex.Match(filename, iisMatchPattern);

            var iisDateMatch = iisMatch.Groups.Values.FirstOrDefault(group => CheckDate(group.Value));

            if(iisDateMatch != null)
            {
                return ParseDate(iisDateMatch.Value);
            }

            return null;
        }

        private static bool CheckDate(string datestring)
        {
            return ParseDate(datestring) != null;
        }

        private static DateTime? ParseDate(string dateString)
        {
            if(DateTime.TryParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return date;
            }

            if (DateTime.TryParseExact(dateString, "yyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var iisDate))
            {
                return iisDate;
            }

            return null;
        }
    }
}
