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
        private FirestoreDb db;

        private const string PROJECT_ID = "help-for-hire";

        public AuthController()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS",
                "C:/Users/busin/Documents/GitHub/help-for-hire-web-api/HelpForHireWebApi/Keys/help-for-hire-firebase-adminsdk-ejiad-ad5b9459ba.json");

            db = FirestoreDb.Create(PROJECT_ID);
        }

        [HttpPost]
        public async Task<IActionResult> PostAuth(Auth auth)
        {
            DocumentReference doc = db.Collection("Auth").Document(auth.Id);

            await doc.SetAsync(auth);

            return CreatedAtAction(nameof(PostAuth), auth);
        }
    }
}
