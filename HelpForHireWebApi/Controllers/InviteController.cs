using Google.Cloud.Firestore;
using HelpForHireWebApi.Managers;
using HelpForHireWebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Controllers
{
    /// <summary>
    /// This controller handles operations related
    /// to invites received by workers from employers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class InviteController : ControllerBase
    {
        /// <summary>
        /// Reference to the collection in the Firestore database
        /// </summary>
        private const string COLLECTION = "Invite";

        /// <summary>
        /// Default constructor
        /// </summary>
        public InviteController()
        {

        }

        /// <summary>
        /// Posts a new invite to the Firestore database
        /// </summary>
        /// <param name="inviteDto">The new invite to post</param>
        /// <returns>Returns the created invite if successful otherwise returns a bad request</returns>
        [HttpPost]
        public async Task<ActionResult<Invite>> PostInvite(InviteDto inviteDto)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Perform a query to check if the invite already exists
            // to prevent duplicates and abuse of the invite system
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("EmployerId", inviteDto.EmployerId)
                .WhereEqualTo("WorkerId", inviteDto.WorkerId);

            // Get a snapshot of the query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // If there is a document, return a bad request to indicate that a duplicate
            // invite was found
            if (querySnapshot.Documents.Count != 0)
            {
                return BadRequest("Duplicate invite detected");
            }

            // Create a reference to the collection
            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            // Create the new invite in the Firestore collection
            await collectionReference.Document().SetAsync(inviteDto);

            return CreatedAtAction(nameof(PostInvite), inviteDto);
        }

        /// <summary>
        /// Gets a specific invite with the specified ID
        /// </summary>
        /// <param name="id">The ID of the invite to retrieve</param>
        /// <returns>The found invite otherwise a not found result</returns>
        [HttpGet]
        public async Task<ActionResult<Invite>> GetInvite(string id)
        {
            // Get a reference to the invite we are searching for
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // Get a snapshot to the invite document
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the document exists, return it, otherwise return not found
            if (documentSnapshot.Exists)
            {
                Invite invite = documentSnapshot.ConvertTo<Invite>();

                invite.InviteId = id;

                return Ok(invite);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets all the invites in the invite collection
        /// </summary>
        /// <returns>Returns a list of invites if any are found otherwise returns not found</returns>
        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Invite>>> GetInvites()
        {
            // List of invite results from the query
            List<Invite> invites = new List<Invite>();

            // Perform a query to retrieve all entries in the Firestore collection
            Query query = FirestoreManager.Db.Collection(COLLECTION);

            // Get a snapshot of this query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // For every retrieved document, convert it to an invite and add it to the results list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Invite invite = documentSnapshot.ConvertTo<Invite>();

                invite.InviteId = documentSnapshot.Id;

                invites.Add(invite);
            }

            // If no invites were found, return a not found result
            if (invites.Count == 0)
            {
                return NotFound();
            }

            return Ok(invites);
        }

        /// <summary>
        /// Updates a specific invite
        /// </summary>
        /// <param name="id">The ID of the invite we want to update</param>
        /// <param name="inviteDto">The new invite information to use in the update</param>
        /// <returns>No content if successful otherwise a not found result</returns>
        [HttpPut]
        public async Task<ActionResult> PutInvite(string id, InviteDto inviteDto)
        {
            // Get a reference to the document we want to update
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // Get a snapshot of the document
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the document exists, we update it otherwise we return not found
            if (documentSnapshot.Exists)
            {
                await documentReference.SetAsync(inviteDto, SetOptions.MergeAll);

                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes an invite
        /// </summary>
        /// <param name="id">The ID of the invite that we want to delete</param>
        /// <returns>No content if successful otherwise a not found result</returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteInvite(string id)
        {
            // Get a reference to the document we want to delete
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // Get a snapshot of the document
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the document exists, then delete it otherwise return not found
            if (documentSnapshot.Exists)
            {
                await documentReference.DeleteAsync();

                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets invites for a specific employer
        /// </summary>
        /// <param name="employerId">The ID of the employer</param>
        /// <returns>A list of invites if any are found otherwise a not found result</returns>
        [HttpGet("/api/[controller]/employer")]
        public async Task<ActionResult<List<Invite>>> GetInvitesForEmployer(string employerId)
        {
            // The list of invites to return
            List<Invite> invites = new List<Invite>();

            // Perform a query to find invites where the employer ID is equal to the one
            // provided to the method
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("EmployerId", employerId);

            // Get a snapshot of the query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // For each found document, convert it to an invite and add it to the invites
            // list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Invite invite = documentSnapshot.ConvertTo<Invite>();

                invite.InviteId = documentSnapshot.Id;

                invites.Add(invite);
            }

            // If no invites were found, return a not found result
            if (invites.Count == 0)
            {
                return NotFound();
            }

            return Ok(invites);
        }

        /// <summary>
        /// Gets invites for a specific worker account
        /// </summary>
        /// <param name="workerId">The ID of the worker</param>
        /// <returns>A list of invites if any are found otherwise a not found result</returns>
        [HttpGet("/api/[controller]/worker")]
        public async Task<ActionResult<List<Invite>>> GetInvitesForWorker(string workerId)
        {
            // List of invites to return
            List<Invite> invites = new List<Invite>();

            // Perform a query to find invites where the worker ID matches the provided
            // ID
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("WorkerId", workerId);

            // Get a snapshot of the query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // Convert each found document into an invite and add it to the results
            // list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Invite invite = documentSnapshot.ConvertTo<Invite>();

                invite.InviteId = documentSnapshot.Id;

                invites.Add(invite);
            }

            // Return not found if no invites were found
            if (invites.Count == 0)
            {
                return NotFound();
            }

            return Ok(invites);
        }
    }
}
