using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    public class Skill
    {
        public int SkillId { get; set; }

        public Worker Worker { get; set; }

        public Job Job { get; set; }
    }
}
