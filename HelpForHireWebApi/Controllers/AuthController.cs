using Google.Cloud.Firestore;
using HelpForHireWebApi.Models;
using HelpForHireWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private FirestoreDb firestoreDb;

        public AuthController()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
                "C:/Users/busin/Documents/GitHub/help-for-hire-web-api/HelpForHireWebApi/Keys/help-for-hire-firebase-adminsdk-ejiad-ad5b9459ba.json");

            firestoreDb = FirestoreDb.Create("help-for-hire");
        }

        [HttpPost]
        public IActionResult PostAuth(Auth auth)
        {
            CollectionReference collectionReference = firestoreDb.Collection("Auth");

            DocumentReference documentReference = collectionReference.AddAsync(auth)
                .GetAwaiter().GetResult();

            return CreatedAtAction(nameof(PostAuth), auth);
        }
    }
}
