using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    /// <summary>
    /// This DTO holds information relevant to history entries for a user
    /// </summary>
    [FirestoreData]
    public class HistoryDto
    {
        /// <summary>
        /// Description of the history entry for a user.
        /// 
        /// An example is: "You registered on 2021/01/01"
        /// </summary>
        [FirestoreProperty]
        [StringLength(256)]
        public string Description { get; set; }

        /// <summary>
        /// Reference to a user which this history entry belongs to.
        /// 
        /// Works for both employers and workers as they both share
        /// the UserId property
        /// </summary>
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public string UserId { get; set; }
    }
}
