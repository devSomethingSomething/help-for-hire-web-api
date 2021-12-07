using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    /// <summary>
    /// The DTO which holds job related information
    /// </summary>
    [FirestoreData]
    public class JobDto
    {
        /// <summary>
        /// The job title.
        /// 
        /// Such as "House Cleaner", "Painter" or "Gardener"
        /// </summary>
        [FirestoreProperty]
        public string Title { get; set; }
    }
}
