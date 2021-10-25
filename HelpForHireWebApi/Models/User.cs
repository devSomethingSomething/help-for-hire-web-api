using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    public abstract class User
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        // public Image ProfilePicture { get; set; }

        public Location Location { get; set; }
    }
}
