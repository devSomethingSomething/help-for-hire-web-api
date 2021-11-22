﻿using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public class Invite
    {
        [Required]
        public string InviteId { get; set; }

        [Required]
        [FirestoreProperty]
        [StringLength(256)]
        public string Status { get; set; }

        [Required]
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public string EmployerId { get; set; }

        [Required]
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public string WorkerId { get; set; }
    }
}
