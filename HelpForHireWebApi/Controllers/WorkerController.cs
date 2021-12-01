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
    public class WorkerController : ControllerBase
    {
        private const string COLLECTION = "Worker";

        public WorkerController()
        {

        }

        [HttpPost]
        public async Task<ActionResult<Worker>> PostWorker(Worker worker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CollectionReference collectionReference = FirestoreManager.Db
                    .Collection(COLLECTION);

            await collectionReference.Document(worker.UserId).SetAsync(worker);

            return CreatedAtAction(nameof(PostWorker), worker);
        }

        [HttpGet]
        public async Task<ActionResult<Worker>> GetWorker(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if (documentSnapshot.Exists)
            {
                Worker worker = documentSnapshot.ConvertTo<Worker>();

                worker.UserId = id;

                return Ok(worker);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Worker>>> GetWorkers()
        {
            List<Worker> workers = new List<Worker>();

            Query query = FirestoreManager.Db.Collection(COLLECTION);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Worker worker = documentSnapshot.ConvertTo<Worker>();

                worker.UserId = documentSnapshot.Id;

                workers.Add(worker);
            }

            if (workers.Count == 0)
            {
                return NotFound();
            }

            return Ok(workers);
        }

        [HttpPut]
        public async Task<ActionResult> PutWorker(string id, WorkerDto workerDto)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if (documentSnapshot.Exists)
            {
                await documentReference.SetAsync(workerDto, SetOptions.MergeAll);

                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteWorker(string id)
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

        [HttpGet("/api/[controller]/cities")]
        public async Task<ActionResult<List<Worker>>> GetWorkersInCity(string locationId)
        {
            List<Worker> workers = new List<Worker>();

            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("LocationId", locationId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Worker worker = documentSnapshot.ConvertTo<Worker>();

                worker.UserId = documentSnapshot.Id;

                workers.Add(worker);
            }

            if (workers.Count == 0)
            {
                return NotFound();
            }

            return Ok(workers);
        }

        // This will change to skills later, for now use the job IDs
        [HttpGet("/api/[controller]/skills")]
        public async Task<ActionResult<List<Worker>>> GetWorkersWithSkills(string locationId, [FromQuery]List<string> jobIds)
        {
            List<Worker> workers = new List<Worker>();

            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("LocationId", locationId)
                .WhereArrayContainsAny("JobIds", jobIds);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Worker worker = documentSnapshot.ConvertTo<Worker>();

                worker.UserId = documentSnapshot.Id;

                workers.Add(worker);
            }

            if (workers.Count == 0)
            {
                return NotFound();
            }

            return Ok(workers);
        }
    }
}
