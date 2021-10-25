using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    public class Employer : User
    {
        public int EmployerId { get; set; }
    }
}
