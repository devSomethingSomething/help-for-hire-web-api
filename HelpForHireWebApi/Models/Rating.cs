using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public string Description { get; set; }

        public Worker Worker { get; set; }

        public Employer Employer { get; set; }
    }
}
