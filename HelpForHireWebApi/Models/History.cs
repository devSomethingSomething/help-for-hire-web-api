using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    public class History
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public User User { get; set; }

        // Not sure if this will work. Cant think of other fields to add
        public Invite Invite { get; set; }

        // We could maybe add previous work reviews?
    }
}
