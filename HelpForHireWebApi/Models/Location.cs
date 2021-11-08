﻿using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public class Location
    {
        [Required]
        public string LocationId { get; set; }

        [Required]
        [FirestoreProperty]
        public string Province { get; set; }

        [Required]
        [FirestoreProperty]
        public string City { get; set; }
    }
}
