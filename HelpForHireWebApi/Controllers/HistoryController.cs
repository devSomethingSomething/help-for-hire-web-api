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
    /// This controller handles operations related to the
    /// history of a user
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : ControllerBase
    {
        /// <summary>
        /// Reference to the collection in the Firestore database
        /// </summary>
        private const string COLLECTION = "History";

        /// <summary>
        /// Default constructor
        /// </summary>
        public HistoryController()
        {

        }

        /// <summary>
        /// Creates a new history document in the Firestore database
        /// </summary>
        /// <param name="historyDto">The history object to be created</param>
        /// <returns>The created history object if successful otherwise a bad request</returns>
        [HttpPost]
        public async Task<ActionResult<History>> PostHistory(HistoryDto historyDto)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a reference to the Firestore collection
            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            // Create the new document in the collection
            await collectionReference.Document().SetAsync(historyDto);

            return CreatedAtAction(nameof(PostHistory), historyDto);
        }

        /// <summary>
        /// Gets a specific history document
        /// </summary>
        /// <param name="id">The ID of the history document we want to retrieve</param>
        /// <returns>The history document if found otherwise a not found result</returns>
        [HttpGet]
        public async Task<ActionResult<History>> GetHistory(string id)
        {
            // Reference to the document we are searching for
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // Get a snapshot of this document
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the document exists, return it otherwise return not found
            if (documentSnapshot.Exists)
            {
                History history = documentSnapshot.ConvertTo<History>();

                history.HistoryId = id;

                return Ok(history);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Get all the history documents in the Firestore database
        /// </summary>
        /// <returns>The list of history documents otherwise a not found result</returns>
        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<History>>> GetHistories()
        {
            // The list of results for history documents
            List<History> histories = new List<History>();

            // Do a query on the history database
            Query query = FirestoreManager.Db.Collection(COLLECTION);

            // Get a snapshot of this query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // For each document found, add it as a history object to the results list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                History history = documentSnapshot.ConvertTo<History>();

                history.HistoryId = documentSnapshot.Id;

                histories.Add(history);
            }

            // If no documents were found, return not found
            if (histories.Count == 0)
            {
                return NotFound();
            }

            return Ok(histories);
        }

        /// <summary>
        /// Updates the specified history document
        /// </summary>
        /// <param name="id">The ID of the history object we want to update</param>
        /// <param name="historyDto">The new history object information</param>
        /// <returns>No content if successful</returns>
        [HttpPut]
        public async Task<ActionResult> PutHistory(string id, HistoryDto historyDto)
        {
            // Create a reference to the history document we want to update
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // --- This should probably change back to how it was before ---
            // Pass through update information
            Dictionary<string, object> updates = new Dictionary<string, object>
            {
                {
                    "Description", historyDto.Description
                }
            };

            // Update the document
            await documentReference.UpdateAsync(updates);

            return NoContent();
        }

        /// <summary>
        /// Deletes a specified history document
        /// </summary>
        /// <param name="id">The ID of the history document we want to delete</param>
        /// <returns>No content if successful otherwise not found</returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteHistory(string id)
        {
            // Create a reference to the document we want to delete
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // If no document was found return not found
            if (documentReference == null)
            {
                return NotFound();
            }

            // Otherwise delete the document
            await documentReference.DeleteAsync();

            return NoContent();
        }

        /// <summary>
        /// Gets all the history related to a specific user
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve history for</param>
        /// <returns>A list of history if any is found otherwise a not found result</returns>
        [HttpGet("/api/[controller]/user")]
        public async Task<ActionResult<List<History>>> GetHistoryByUser(string userId)
        {
            // List of results to return
            List<History> histories = new List<History>();

            // Perform a query to find any history which belongs to a user
            // with the provided ID
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("UserId", userId);

            // Get a snapshot of this query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // For each found document, create a history object and add it to the
            // results list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                History history = documentSnapshot.ConvertTo<History>();

                history.HistoryId = documentSnapshot.Id;

                histories.Add(history);
            }

            // If no history was found for a user, return not found
            if (histories.Count == 0)
            {
                return NotFound();
            }

            return Ok(histories);
        }

        /// <summary>
        /// Deletes all the history for a specific user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("/api/[controller]/all")]
        public async Task<ActionResult> DeleteAllHistoryForUser(string userId)
        {
            // Perform a query to find all documents where the user ID matches
            // the one provided to the method
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("UserId", userId);

            // Get a snapshot of this query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // Delete all found documents
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                await documentSnapshot.Reference.DeleteAsync();
            }

            return NoContent();
        }
    }
}
