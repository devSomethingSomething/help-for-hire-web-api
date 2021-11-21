using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public abstract class UserDto
    {
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Surname { get; set; }

        [FirestoreProperty]
        [StringLength(10, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [FirestoreProperty]
        public string LocationId { get; set; }
    }
}
