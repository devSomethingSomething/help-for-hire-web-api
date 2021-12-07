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
    /// This controller handles operations related to
    /// locations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : ControllerBase
    {
        /// <summary>
        /// Reference to the collection in the Firestore database
        /// </summary>
        private const string COLLECTION = "Location";

        /// <summary>
        /// Default constructor
        /// </summary>
        public LocationController()
        {

        }

        /// <summary>
        /// This method posts a new location
        /// </summary>
        /// <param name="locationDto">The location we want to post</param>
        /// <returns>The created location, but if it is not valid then a bad request</returns>
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(LocationDto locationDto)
        {
            // Check if model is valid first
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a reference to the collection
            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            // Create the new location
            await collectionReference.Document().SetAsync(locationDto);

            return CreatedAtAction(nameof(PostLocation), locationDto);
        }

        /// <summary>
        /// Gets one location
        /// </summary>
        /// <param name="id">The ID of the location to be retrieved</param>
        /// <returns>A location otherwise a not found result if no location was found</returns>
        [HttpGet]
        public async Task<ActionResult<Location>> GetLocation(string id)
        {
            // Create a reference to the document we are searching for
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // Get a document snapshot
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the location exists, return it, otherwise return not found
            if (documentSnapshot.Exists)
            {
                Location location = documentSnapshot.ConvertTo<Location>();

                location.LocationId = id;

                return Ok(location);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets all the locations in the Firestore database
        /// </summary>
        /// <returns>A list of locations otherwise a not found result</returns>
        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Location>>> GetLocations()
        {
            // List of locations to return
            List<Location> locations = new List<Location>();

            // Get all the locations in the collection
            Query query = FirestoreManager.Db.Collection(COLLECTION);

            // Get a snapshot of the query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // For each found document, add it to the results list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Location location = documentSnapshot.ConvertTo<Location>();

                location.LocationId = documentSnapshot.Id;

                locations.Add(location);
            }

            // If no locations were found, return not found
            if (locations.Count == 0)
            {
                return NotFound();
            }

            return Ok(locations);
        }

        /// <summary>
        /// Updates a single location
        /// </summary>
        /// <param name="id">The ID of the location to update</param>
        /// <param name="locationDto">The new location information to use in the update</param>
        /// <returns>No content if successful otherwise not found</returns>
        [HttpPut]
        public async Task<ActionResult> PutLocation(string id, LocationDto locationDto)
        {
            // Get a reference to the document we want to update
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // If no document was found, return not found
            if (documentReference == null)
            {
                return NotFound();
            }

            // Otherwise update the document with the new location information
            await documentReference.SetAsync(locationDto, SetOptions.MergeAll);

            return NoContent();
        }

        /// <summary>
        /// Deletes a single location
        /// </summary>
        /// <param name="id">The ID of the location to delete</param>
        /// <returns>No content if successful otherwise not found</returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteLocation(string id)
        {
            // Get a reference to the location to be deleted
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // If the location is not found, return not found
            if (documentReference == null)
            {
                return NotFound();
            }

            // Otherwise delete the location document
            await documentReference.DeleteAsync();

            return NoContent();
        }

        /// <summary>
        /// Get cities in a selected province
        /// </summary>
        /// <param name="province">The province to be used in the search</param>
        /// <returns>A list of locations in a province otherwise not found</returns>
        [HttpGet("/api/[controller]/cities")]
        public async Task<ActionResult<List<Location>>> GetCitiesInProvince(string province)
        {
            // The list of locations to return
            List<Location> locations = new List<Location>();

            // Perform a query to get the locations where the province matches
            // the provided province
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("Province", province);

            // Get a snapshot of the query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // For any found locations, add them to the list of results
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Location location = documentSnapshot.ConvertTo<Location>();

                location.LocationId = documentSnapshot.Id;

                locations.Add(location);
            }

            // If no locations were found, return not found
            if (locations.Count == 0)
            {
                return NotFound();
            }

            return Ok(locations);
        }
    }
}
