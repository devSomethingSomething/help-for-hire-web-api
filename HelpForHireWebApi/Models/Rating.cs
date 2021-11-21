using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public class Rating
    {
        [Required]
        public string RatingId { get; set; }

        [Required]
        [FirestoreProperty]
        [Range(1.0, 10.0)]
        public int Value { get; set; }

        [Required]
        [FirestoreProperty]
        [StringLength(256)]
        public string Description { get; set; }

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
