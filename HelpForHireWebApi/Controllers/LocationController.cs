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
    public class LocationController : ControllerBase
    {
        const string COLLECTION = "Location";

        public LocationController()
        {

        }

        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(LocationDto locationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            await collectionReference.Document().SetAsync(locationDto);

            return CreatedAtAction(nameof(PostLocation), locationDto);
        }

        [HttpGet]
        public async Task<ActionResult<Location>> GetLocation(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

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

        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Location>>> GetLocations()
        {
            List<Location> locations = new List<Location>();

            Query query = FirestoreManager.Db.Collection(COLLECTION);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Location location = documentSnapshot.ConvertTo<Location>();

                location.LocationId = documentSnapshot.Id;

                locations.Add(location);
            }

            if (locations.Count == 0)
            {
                return NotFound();
            }

            return Ok(locations);
        }

        [HttpPut]
        public async Task<ActionResult> PutLocation(string id, LocationDto locationDto)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            if (documentReference is null)
            {
                return NotFound();
            }

            await documentReference.SetAsync(locationDto, SetOptions.MergeAll);

            return NoContent();
        }
    }
}
