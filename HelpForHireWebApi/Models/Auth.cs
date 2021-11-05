using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public class Auth
    {
        [Required]
        [StringLength(13, MinimumLength = 13)]
        public string Id { get; set; }

        [Required]
        [FirestoreProperty]
        [StringLength(24, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
