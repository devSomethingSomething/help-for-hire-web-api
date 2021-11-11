using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Managers
{
    public static class FirestoreManager
    {
        public static FirestoreDb Db { get; set; }

        private const string PROJECT_ID = "help-for-hire";

        static FirestoreManager()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
                Path.Combine(
                    Environment.CurrentDirectory, 
                    "Keys/", 
                    "help-for-hire-firebase-adminsdk-ejiad-ad5b9459ba.json"));

            Db = FirestoreDb.Create(PROJECT_ID);
        }
    }
}
