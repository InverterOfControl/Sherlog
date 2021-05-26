using System;
using System.Collections.Generic;
using System.Text;

namespace Sherlog.Shared.Models
{
    public class GroupedLogModel
    {
        public string LogTypeName { get; set; }

        public IEnumerable<string> Filepaths { get; set; }

        public string Timerange { get; set; }
    }
}
