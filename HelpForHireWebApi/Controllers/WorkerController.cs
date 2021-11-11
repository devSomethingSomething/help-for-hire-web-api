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
    [Route("api/[controller]")]
    [ApiController]

    public class WorkerController : ControllerBase
    {

        public WorkerController()
        {

        }

        [HttpGet]
        public async Task<ActionResult<Worker>> getWorker(string UserId)
        {
            DocumentReference documentReference = FirestoreManager.Db.Collection("Worker").Document(UserId);
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if(documentSnapshot.Exists){
                Worker worker = documentSnapshot.ConvertTo<Worker>();
                return Ok(worker);
            }
            else
            {
                return NotFound();
            } 
        }

        [HttpPost]
        public async Task<ActionResult> postWorker(Worker worker)
        {
            DocumentReference documentReference = FirestoreManager.Db.Collection("Worker").Document(worker.UserId);

            await documentReference.SetAsync(worker);

            return CreatedAtAction(nameof(postWorker), worker);
        }

        [HttpPut]
        public async Task<ActionResult> putWorker(string userId,Worker worker)
        {

            if (userId != worker.UserId)
            {
                return BadRequest();
            }

            DocumentReference documentReference = FirestoreManager.Db.Collection("worker").Document(userId);

            await documentReference.SetAsync(worker);

            return NoContent();
        }
    }
}
