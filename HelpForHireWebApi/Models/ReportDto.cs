using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Models
{
    [FirestoreData]
    public class ReportDto
    {
       
        
        [FirestoreProperty]
        public string ReportType { get; set; }
        
        [FirestoreProperty]
        [StringLength(256)]
        public string Description { get; set; }
        
        [FirestoreProperty]
        [StringLength(13, MinimumLength = 13)]
        public User User { get; set; }
    }
}
