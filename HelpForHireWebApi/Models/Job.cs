﻿using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public class Job
    {
        [Required]
        public string JobId { get; set; }

        [Required]
        [FirestoreProperty]
        public string Title { get; set; }
    }
}
