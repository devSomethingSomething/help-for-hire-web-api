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

        /// <summary>
        /// Get method for the auth controller
        /// Returns an auth object from firestore
        /// </summary>
        /// <param name="id">Auth id</param>
        /// <returns>Request result</returns>
        [HttpGet]
        public async Task<ActionResult<Auth>> GetAuth(string id)
        {
            // Get a reference to a specific document in the auth collection
            DocumentReference documentReference = FirestoreManager.Db.Collection("Auth").Document(id);

            // Get a snapshot of the referenced document
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // Check if the referenced document exists before anything else occurs
            if (documentSnapshot.Exists)
            {
                // Create a new auth object based off of the document we previously
                // retrieved from firestore
                Auth auth = documentSnapshot.ConvertTo<Auth>();

                // Make sure to set the id on the new model, as it is not marked as
                // a firestore property, meaning that it won't be automatically set
                auth.Id = id;

                // Return an okay result along with the retrieved auth object from
                // the firestore db
                return Ok(auth);
            }
            else
            {
                // If the document does not exist, return a not found result
                return NotFound();
            }
        }

        /// <summary>
        /// Post method for the auth controller
        /// Adds an auth object to firestore
        /// </summary>
        /// <param name="auth">The auth object we want to post</param>
        /// <returns>Request result</returns>
        [HttpPost]
        public async Task<ActionResult> PostAuth(Auth auth)
        {
            // Reference a document in firestore, will be created if it does not
            // yet exist in the db
            DocumentReference documentReference = FirestoreManager.Db.Collection("Auth").Document(auth.Id);

            // Wait while the new auth object is being added to firestore
            await documentReference.SetAsync(auth);

            // Return a completed result and the posted object
            return CreatedAtAction(nameof(PostAuth), auth);
        }

        /// <summary>
        /// Put method for the auth controller
        /// Updates an existing auth object in the firestore db
        /// </summary>
        /// <param name="id">The id of the auth object to update</param>
        /// <param name="auth">The new auth object data to use for the update</param>
        /// <returns>Request result</returns>
        [HttpPut]
        public async Task<ActionResult> PutAuth(string id, Auth auth)
        {
            // Firstly check if the ids do match, otherwise it wouldn't make
            // sense to do the update
            // --- This could probably be improved upon ---
            if (id != auth.Id)
            {
                // Return a bad request if the ids do not match
                return BadRequest();
            }

            // Get a reference to the firestore document to update
            DocumentReference documentReference = FirestoreManager.Db.Collection("Auth").Document(id);

            // Check if the document does exist
            if (documentReference is null)
            {
                // Return a not found to indicate that the document does not exist
                // or that it could not be found
                return NotFound();
            }

            // Set the existing document with the new auth data, will overwrite
            // the old data in the process
            await documentReference.SetAsync(auth);

            // Return a no content result to indicate a successful update
            return NoContent();
        }
    }
}
