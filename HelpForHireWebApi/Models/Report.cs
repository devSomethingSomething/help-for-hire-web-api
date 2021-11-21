using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public class Report
    {
        [Required]
        public string ReportId { get; set; }

        [Required]
        [FirestoreProperty]
        [StringLength(256)]
        public string ReportType { get; set; }

        [Required]
        [FirestoreProperty]
        [StringLength(256)]
        public string Description { get; set; }

        [Required]
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public string UserId { get; set; }
    }
}
