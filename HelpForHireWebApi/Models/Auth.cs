using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public class Auth
    {
        public string Id { get; set; }

        [FirestoreProperty]
        public string Password { get; set; }
    }
}
