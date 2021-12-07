using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    /// <summary>
    /// This class holds skills for workers
    /// </summary>
    [FirestoreData]
    [Obsolete("Currently not needed")]
    public class Skill
    {
        /// <summary>
        /// Auto-generated ID for each skill
        /// </summary>
        [Required]
        public string SkillId { get; set; }

        /// <summary>
        /// User to which this skill belongs
        /// </summary>
        [Required]
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public string UserId { get; set; }

        /// <summary>
        /// The actual job which a user can perform
        /// </summary>
        [Required]
        [FirestoreProperty]
        public string JobId { get; set; }
    }
}
