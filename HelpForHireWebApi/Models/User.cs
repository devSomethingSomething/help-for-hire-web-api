using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public abstract class User
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [FirestoreProperty]
        public string Name { get; set; }

        [Required]
        [FirestoreProperty]
        public string Surname { get; set; }

        [Required]
        [FirestoreProperty]
        public string PhoneNumber { get; set; }

        // public Image ProfilePicture { get; set; }

        [Required]
        [FirestoreProperty]
        public DocumentReference LocationId { get; set; }
    }
}
