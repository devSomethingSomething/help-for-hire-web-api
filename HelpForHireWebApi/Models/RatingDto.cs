using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    /// <summary>
    /// The DTO used for updating and posting ratings
    /// </summary>
    [FirestoreData]
    public class RatingDto
    {
        /// <summary>
        /// A value between 1 and 5, used by a
        /// star rating widget in the Flutter
        /// application
        /// </summary>
        [FirestoreProperty]
        [Range(1.0, 5.0)]
        public int Value { get; set; }

        /// <summary>
        /// A short description as extra information
        /// for each rating.
        /// 
        /// Is optional, a user does not have to provide
        /// a description. If no description is
        /// provided, the value will just be an empty
        /// string
        /// </summary>
        [FirestoreProperty]
        [StringLength(256)]
        public string Description { get; set; }

        /// <summary>
        /// The employer who posted the rating
        /// </summary>
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public string EmployerId { get; set; }

        /// <summary>
        /// The worker who received the rating
        /// </summary>
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public string WorkerId { get; set; }
    }
}
