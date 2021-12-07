using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    /// <summary>
    /// This model holds information related to reports
    /// against a user
    /// </summary>
    [FirestoreData]
    public class Report
    {
        /// <summary>
        /// Auto-generated ID for each report
        /// </summary>
        [Required]
        public string ReportId { get; set; }

        /// <summary>
        /// The type of report being made.
        /// 
        /// Examples are "Scammer" or "Non-compliance"
        /// </summary>
        [Required]
        [FirestoreProperty]
        [StringLength(256)]
        public string ReportType { get; set; }

        /// <summary>
        /// A short description with extra information
        /// which is useful to backup the report
        /// </summary>
        [Required]
        [FirestoreProperty]
        [StringLength(256)]
        public string Description { get; set; }

        /// <summary>
        /// The user who is being reported
        /// </summary>
        [Required]
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public string ReportedUserId { get; set; }

        /// <summary>
        /// The user who is making the report
        /// </summary>
        [Required]
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public string ReporterUserId { get; set; }
    }
}
