using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    /// <summary>
    /// This DTO handles location information for updates
    /// and posts
    /// </summary>
    [FirestoreData]
    public class LocationDto
    {
        /// <summary>
        /// Province for each city
        /// </summary>
        [FirestoreProperty]
        public string Province { get; set; }

        /// <summary>
        /// Cities stored in the Firestore database
        /// </summary>
        [FirestoreProperty]
        public string City { get; set; }
    }
}
