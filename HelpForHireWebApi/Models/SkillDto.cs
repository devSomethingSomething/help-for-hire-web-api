using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    /// <summary>
    /// The DTO for skills a user can perform
    /// </summary>
    [FirestoreData]
    [Obsolete("Currently not needed")]
    public class SkillDto
    {
        /// <summary>
        /// User to which this skill belongs
        /// </summary>
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public string UserId { get; set; }

        /// <summary>
        /// The actual job which a user can perform
        /// </summary>
        [FirestoreProperty]
        public string JobId { get; set; }
    }
}
