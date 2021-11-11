using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public class WorkerDto : UserDto
    {
        [FirestoreProperty]
        [StringLength(256)]
        public string Description { get; set; }

        [FirestoreProperty]
        [DefaultValue(500)]
        [Range(0.0, 100000.0)]
        public int MinimumFee { get; set; }

        [FirestoreProperty]
        [DefaultValue(false)]
        public bool FullTime { get; set; }

        [FirestoreProperty]
        [DefaultValue(false)]
        public bool PartTime { get; set; }

        [FirestoreProperty]
        public List<string> JobIds { get; set; }
    }
}
