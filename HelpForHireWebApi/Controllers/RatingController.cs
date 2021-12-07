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
    /// ratings or reviews.
    /// 
    /// These are ratings posted by employers on worker
    /// account types
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        /// <summary>
        /// Reference to the collection in the Firestore database
        /// </summary>
        private const string COLLECTION = "Rating";

        /// <summary>
        /// Default constructor
        /// </summary>
        public RatingController()
        {

        }

        /// <summary>
        /// Creates a new rating in the Firestore database
        /// </summary>
        /// <param name="ratingDto">The rating data to post</param>
        /// <returns>The created rating otherwise a bad request</returns>
        [HttpPost]
        public async Task<ActionResult<Rating>> PostRating(RatingDto ratingDto)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check for duplicate ratings
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("EmployerId", ratingDto.EmployerId)
                .WhereEqualTo("WorkerId", ratingDto.WorkerId);

            // Get a snapshot of the query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // Return a bad request if a duplicate rating was found
            if (querySnapshot.Documents.Count != 0)
            {
                return BadRequest("Duplicate rating detected");
            }

            // Get a reference to the collection
            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            // Create the new rating in the database
            await collectionReference.Document().SetAsync(ratingDto);

            return CreatedAtAction(nameof(PostRating), ratingDto);
        }

        /// <summary>
        /// Gets a single rating
        /// </summary>
        /// <param name="id">The ID of the rating we want to retrieve</param>
        /// <returns>A rating if found or a not found result</returns>
        [HttpGet]
        public async Task<ActionResult<Rating>> GetRating(string id)
        {
            // Get a reference to the rating to be retrieved
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // Get a snapshot for the rating
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the rating exists, return it, otherwise return not found
            if (documentSnapshot.Exists)
            {
                Rating rating = documentSnapshot.ConvertTo<Rating>();

                rating.RatingId = id;

                return Ok(rating);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets all the ratings in the database
        /// </summary>
        /// <returns>A list of ratings if any exist otherwise a not found result</returns>
        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Rating>>> GetRatings()
        {
            // List of ratings to return
            List<Rating> ratings = new List<Rating>();

            // Get all ratings
            Query query = FirestoreManager.Db.Collection(COLLECTION);

            // Get a snapshot of the query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // Add each found rating to the ratings list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Rating rating = documentSnapshot.ConvertTo<Rating>();

                rating.RatingId = documentSnapshot.Id;

                ratings.Add(rating);
            }

            // If no ratings were found, return not found
            if (ratings.Count == 0)
            {
                return NotFound();
            }

            return Ok(ratings);
        }

        /// <summary>
        /// Updates a single rating
        /// </summary>
        /// <param name="id">The ID of the rating to update</param>
        /// <param name="ratingDto">The new rating data to use in the update</param>
        /// <returns>No content if the update worked otherwise a not found result</returns>
        [HttpPut]
        public async Task<ActionResult> PutRating(string id, RatingDto ratingDto)
        {
            // Get a reference to the rating to be updated
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // Get a snapshot of the rating
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the rating exists, update it, otherwise return not found
            if (documentSnapshot.Exists)
            {
                await documentReference.SetAsync(ratingDto, SetOptions.MergeAll);

                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes a single rating from the database
        /// </summary>
        /// <param name="id">The ID of the rating to delete</param>
        /// <returns>No content if successful otherwise not found</returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteRating(string id)
        {
            // Get a reference to the rating to be deleted
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // Get a snapshot to the rating
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the rating exists, delete it, otherwise return not found
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
        /// Gets the average rating for a worker
        /// </summary>
        /// <param name="workerId">The ID of the worker which we want to calculate the average for</param>
        /// <returns>A value for the average rating, one if no ratings were found</returns>
        [HttpGet("/api/[controller]/worker")]
        public async Task<ActionResult<int>> GetAverageRatingForWorker(string workerId)
        {
            int ratingsSum = 0;

            int numberOfRatings = 0;

            // Find all ratings for the worker with the specified ID
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("WorkerId", workerId);

            // Get a snapshot of the query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // Use each document to increment the sum of the ratings
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                ratingsSum += documentSnapshot.ConvertTo<Rating>().Value;

                numberOfRatings++;
            }

            // If no ratings were found, return one
            if (ratingsSum == 0)
            {
                return 1;
            }

            // Return the average rating for the specified worker
            return Ok(ratingsSum / numberOfRatings);
        }
    }
}
