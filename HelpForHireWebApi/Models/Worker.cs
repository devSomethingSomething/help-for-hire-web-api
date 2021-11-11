﻿using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public class Worker : User
    {
        [Required]
        [FirestoreProperty]
        [StringLength(256)]
        public string Description { get; set; }

        [Required]
        [FirestoreProperty]
        [DefaultValue(500)]
        [Range(0.0, 100000.0)]
        public int MinimumFee { get; set; }

        [Required]
        [FirestoreProperty]
        [DefaultValue(false)]
        public bool FullTime { get; set; }

        [Required]
        [FirestoreProperty]
        [DefaultValue(false)]
        public bool PartTime { get; set; }

        [Required]
        [FirestoreProperty]
        public List<string> JobIds { get; set; }
    }
}
