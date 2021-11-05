using Google.Cloud.Firestore;
using HelpForHireWebApi.Managers;
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
        public AuthController()
        {

        }

        [HttpGet]
        public async Task<ActionResult<Auth>> GetAuth(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db.Collection("Auth").Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if (documentSnapshot.Exists)
            {
                Auth auth = documentSnapshot.ConvertTo<Auth>();

                auth.Id = id;

                return Ok(auth);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostAuth(Auth auth)
        {
            DocumentReference documentReference = FirestoreManager.Db.Collection("Auth").Document(auth.Id);

            await documentReference.SetAsync(auth);

            return CreatedAtAction(nameof(PostAuth), auth);
        }
        
        // Need put method here
    }
}
