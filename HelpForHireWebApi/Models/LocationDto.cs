using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public class LocationDto
    {
        [FirestoreProperty]
        public string Province { get; set; }

        [FirestoreProperty]
        public string City { get; set; }
    }
}
