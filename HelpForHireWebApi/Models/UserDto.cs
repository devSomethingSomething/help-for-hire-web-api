using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    /// <summary>
    /// DTO used for updating users
    /// </summary>
    [FirestoreData]
    public abstract class UserDto
    {
        /// <summary>
        /// The name of the user
        /// </summary>
        [FirestoreProperty]
        public string Name { get; set; }

        /// <summary>
        /// The surname of the user
        /// </summary>
        [FirestoreProperty]
        public string Surname { get; set; }

        /// <summary>
        /// The phone number of the user
        /// </summary>
        [FirestoreProperty]
        [StringLength(10, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The location of the user.
        /// 
        /// Used in finding profiles according to the city
        /// where a user stays
        /// </summary>
        [FirestoreProperty]
        public string LocationId { get; set; }
    }
}
