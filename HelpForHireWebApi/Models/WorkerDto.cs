using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    /// <summary>
    /// DTO used for updating worker accounts
    /// </summary>
    [FirestoreData]
    public class WorkerDto : UserDto
    {
        /// <summary>
        /// Optional description for each worker
        /// </summary>
        [FirestoreProperty]
        [StringLength(256)]
        public string Description { get; set; }

        /// <summary>
        /// This is what a worker charges per day
        /// </summary>
        [FirestoreProperty]
        [DefaultValue(500)]
        [Range(0.0, 100000.0)]
        public int MinimumFee { get; set; }

        /// <summary>
        /// Whether or not a worker is looking for
        /// full time work
        /// </summary>
        [FirestoreProperty]
        [DefaultValue(false)]
        public bool FullTime { get; set; }

        /// <summary>
        /// Whether or not a worker is looking for
        /// part time work
        /// </summary>
        [FirestoreProperty]
        [DefaultValue(false)]
        public bool PartTime { get; set; }

        /// <summary>
        /// The various jobs a worker can perform
        /// </summary>
        [FirestoreProperty]
        public List<string> JobIds { get; set; }
    }
}
