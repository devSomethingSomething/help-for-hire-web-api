using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    /// <summary>
    /// This model holds information relevant to an employer
    /// </summary>
    [FirestoreData]
    public class Employer : User
    {
        /// <summary>
        /// Optional company name.
        /// 
        /// The Flutter application will set this to N/A
        /// if no company name is provided
        /// </summary>
        [Required]
        [FirestoreProperty]
        [StringLength(256)]
        public string CompanyName { get; set; }

        /// <summary>
        /// Required address of the employer.
        /// 
        /// Does not require a specific format to be acceptable
        /// for the Firestore database
        /// </summary>
        [Required]
        [FirestoreProperty]
        [StringLength(256)]
        public string Address { get; set; }

        /// <summary>
        /// Required suburb of the employer.
        /// 
        /// Does not require a specific kind of format,
        /// only that the string is not empty
        /// </summary>
        [Required]
        [FirestoreProperty]
        [StringLength(256)]
        public string Suburb { get; set; }
    }
}
