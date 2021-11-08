﻿using Google.Cloud.Firestore;
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
    }
}
