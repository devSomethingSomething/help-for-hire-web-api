﻿using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public class Employer : User
    {
        [FirestoreProperty]
        [StringLength(256)]
        public string CompanyName { get; set; }

        [FirestoreProperty]
        [StringLength(256)]
        public string Address { get; set; }

        [FirestoreProperty]
        [StringLength(256)]
        public string Suburb { get; set; }
    }
}
