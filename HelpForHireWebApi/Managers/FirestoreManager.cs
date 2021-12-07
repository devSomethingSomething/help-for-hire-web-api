using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Managers
{
    /// <summary>
    /// This class handles references to Firestore
    /// to ensure that they are only created once and then
    /// used throughout the application
    /// </summary>
    public static class FirestoreManager
    {
        /// <summary>
        /// Reference to the Firestore database
        /// </summary>
        public static FirestoreDb Db { get; set; }

        /// <summary>
        /// Entry point of the Firebase application
        /// </summary>
        public static FirebaseApp App { get; set; }

        /// <summary>
        /// Entry point for Firebase Auth operations
        /// </summary>
        public static FirebaseAuth Auth { get; set; }

        /// <summary>
        /// Project ID in Firebase.
        /// 
        /// Usually a name like "help-for-hire-12345"
        /// </summary>
        private const string PROJECT_ID = "help-for-hire";

        /// <summary>
        /// Constructor for this manager class
        /// </summary>
        static FirestoreManager()
        {
            // Set the Google credentials to the appropriate environment variable
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
                Path.Combine(
                    Environment.CurrentDirectory, 
                    "Keys/", 
                    "help-for-hire-firebase-adminsdk-ejiad-ad5b9459ba.json"));

            // Initialize the database reference
            Db = FirestoreDb.Create(PROJECT_ID);

            // Initialize the application reference
            App = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });

            // Set the Auth reference variable to the default Firebase instance
            Auth = FirebaseAuth.DefaultInstance;
        }
    }
}
