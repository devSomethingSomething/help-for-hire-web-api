using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    /// <summary>
    /// The DTO for invites
    /// </summary>
    [FirestoreData]
    public class InviteDto
    {
        /// <summary>
        /// The current status of the invite.
        /// 
        /// An invite may be accepted, declined or pending
        /// </summary>
        [FirestoreProperty]
        [StringLength(256)]
        public string Status { get; set; }

        /// <summary>
        /// The employer to which this invite belongs
        /// </summary>
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public string EmployerId { get; set; }

        /// <summary>
        /// The worker to which this invite was sent
        /// </summary>
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public string WorkerId { get; set; }
    }
}
