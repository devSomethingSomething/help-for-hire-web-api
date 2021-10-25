using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    public class Worker : User
    {
        // This might be useless since we already inherit an id
        public int WorkerId { get; set; }

        public string Description { get; set; }

        public int MinimumFee { get; set; }

        public bool FullTime { get; set; }

        public bool PartTime { get; set; }

        public List<Job> Jobs { get; set; }
    }
}
