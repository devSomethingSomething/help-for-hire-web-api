using Google.Cloud.Firestore;
using HelpForHireWebApi.Managers;
using HelpForHireWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class RatingController : Controller
    {
        public RatingController()
        {

        }

        [HttpPost]
        public async Task<ActionResult<Rating>> PostRating(Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CollectionReference collectionReference = FirestoreManager.Db
                .Collection("Rating");

            await collectionReference.Document().SetAsync(rating);

            return CreatedAtAction(nameof(PostRating), rating);
        }

        [HttpGet]
        public async Task<ActionResult<Rating>> GetRating(int id)
        {
            DocumentReference documentReference = FirestoreManager.Db.Collection("Rating").Document(id.ToString());

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if(documentSnapshot.Exists)
            {
                Rating rating = documentSnapshot.ConvertTo<Rating>();

                rating.Id = id;

                return Ok(rating);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPut]
        public async Task<ActionResult> PutRating(string id, Rating rating)
        {
            DocumentReference documentReference = FirestoreManager.Db.Collection("Rating").Document(id);

            if(documentReference == null)
            {
                return NotFound();
            }

            await documentReference.SetAsync(rating, SetOptions.MergeAll);
            return NoContent();

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteRating(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db.Collection("Rating").Document(id);

            if (documentReference == null)
            {
                return NotFound();
            }

            await documentReference.DeleteAsync();
            return NoContent();
        }
    }
}
