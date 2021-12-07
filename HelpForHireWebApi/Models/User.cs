using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    /// <summary>
    /// The user class which employers and workers
    /// inherit from.
    /// 
    /// Holds information shared between the two types
    /// of users such as IDs
    /// </summary>
    [FirestoreData]
    public abstract class User
    {
        /// <summary>
        /// The ID of the user who is using the
        /// application, not auto-generated
        /// </summary>
        [Required]
        [StringLength(13, MinimumLength = 13)]
        public string UserId { get; set; }

        /// <summary>
        /// The name of the user
        /// </summary>
        [Required]
        [FirestoreProperty]
        public string Name { get; set; }

        /// <summary>
        /// The surname of the user
        /// </summary>
        [Required]
        [FirestoreProperty]
        public string Surname { get; set; }

        /// <summary>
        /// The phone number of the user
        /// </summary>
        [Required]
        [FirestoreProperty]
        [StringLength(10, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The location of the user.
        /// 
        /// Used in finding profiles according to the city
        /// where a user stays
        /// </summary>
        [Required]
        [FirestoreProperty]
        public string LocationId { get; set; }
    }
}
