using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    /// <summary>
    /// This model holds information related to
    /// locations. These locations are then
    /// referenced by users to indicate their
    /// city
    /// </summary>
    [FirestoreData]
    public class Location
    {
        /// <summary>
        /// Auto-generated ID for each location
        /// </summary>
        [Required]
        public string LocationId { get; set; }
        
        /// <summary>
        /// Province for each city
        /// </summary>
        [Required]
        [FirestoreProperty]
        public string Province { get; set; }

        /// <summary>
        /// Cities stored in the Firestore database
        /// </summary>
        [Required]
        [FirestoreProperty]
        public string City { get; set; }
    }
}
