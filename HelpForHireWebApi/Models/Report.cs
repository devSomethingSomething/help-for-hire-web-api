using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    public class Report
    {
        public int Id { get; set; }

        public ReportType ReportType { get; set; }

        public string Description { get; set; }

        public User User { get; set; }
    }
}
