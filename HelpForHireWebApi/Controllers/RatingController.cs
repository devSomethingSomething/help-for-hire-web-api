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
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private const string COLLECTION = "Rating";

        public RatingController()
        {

        }

        [HttpPost]
        public async Task<ActionResult<Rating>> PostRating(RatingDto ratingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            await collectionReference.Document().SetAsync(ratingDto);

            return CreatedAtAction(nameof(PostRating), ratingDto);
        }

        [HttpGet]
        public async Task<ActionResult<Rating>> GetRating(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

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

        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Rating>>> GetRatings()
        {
            List<Rating> ratings = new List<Rating>();

            Query query = FirestoreManager.Db.Collection(COLLECTION);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Rating rating = documentSnapshot.ConvertTo<Rating>();

                rating.RatingId = documentSnapshot.Id;

                ratings.Add(rating);
            }

            if (ratings.Count == 0)
            {
                return NotFound();
            }

            return Ok(ratings);
        }

        [HttpPut]
        public async Task<ActionResult> PutRating(string id, RatingDto ratingDto)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

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

        [HttpDelete]
        public async Task<ActionResult> DeleteRating(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

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

        [HttpGet("/api/[controller]/worker")]
        public async Task<ActionResult<int>> GetAverageRatingForWorker(string workerId)
        {
            int ratingsSum = 0;

            int numberOfRatings = 0;

            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("WorkerId", workerId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                ratingsSum += documentSnapshot.ConvertTo<Rating>().Value;

                numberOfRatings++;
            }

            if (ratingsSum == 0)
            {
                return 0;
            }

            return Ok(ratingsSum / numberOfRatings);
        }
    }
}
