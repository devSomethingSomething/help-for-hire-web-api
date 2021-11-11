using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    [Obsolete("Currently not needed")]
    public class SkillDto
    {
        [Required]
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public string UserId { get; set; }

        [Required]
        [FirestoreProperty]
        public string JobId { get; set; }
    }
}
