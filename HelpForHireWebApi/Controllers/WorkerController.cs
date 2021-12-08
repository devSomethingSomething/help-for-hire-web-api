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
    /// This controller handles operations for worker
    /// account types
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerController : ControllerBase
    {
        /// <summary>
        /// Reference to the collection in the Firestore database
        /// </summary>
        private const string COLLECTION = "Worker";

        /// <summary>
        /// Default constructor
        /// </summary>
        public WorkerController()
        {

        }

        /// <summary>
        /// Creates a new worker in the Firestore database
        /// </summary>
        /// <param name="worker">The new worker to be posted</param>
        /// <returns>The newly created worker otherwise a bad request if there is an error</returns>
        [HttpPost]
        public async Task<ActionResult<Worker>> PostWorker(Worker worker)
        {
            // If the model is not valid, then return a bad request with the error
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CollectionReference collectionReference = FirestoreManager.Db
                    .Collection(COLLECTION);

            // Create the new worker in the database
            await collectionReference.Document(worker.UserId).SetAsync(worker);

            // Return the created worker object
            return CreatedAtAction(nameof(PostWorker), worker);
        }

        /// <summary>
        /// Gets a single worker object back
        /// </summary>
        /// <param name="id">The ID of the worker to retrieve</param>
        /// <returns>A worker object if found, otherwise a not found result</returns>
        [HttpGet]
        public async Task<ActionResult<Worker>> GetWorker(string id)
        {
            // Attempt to find the worker document
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the worker document exists, return a worker object, otherwise
            // return a not found result
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

        /// <summary>
        /// Gets all the worker objects from the database
        /// </summary>
        /// <returns>A list of worker objects if any are found, otherwise a not found result</returns>
        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Worker>>> GetWorkers()
        {
            // The list of workers to return
            List<Worker> workers = new List<Worker>();

            // Perform a query to retrieve all the worker documents
            Query query = FirestoreManager.Db.Collection(COLLECTION);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // Convert each found document to a worker object and add it to the results list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Worker worker = documentSnapshot.ConvertTo<Worker>();

                worker.UserId = documentSnapshot.Id;

                workers.Add(worker);
            }

            // Return a not found result if no worker documents were found
            if (workers.Count == 0)
            {
                return NotFound();
            }

            // Return any found worker objects
            return Ok(workers);
        }

        /// <summary>
        /// Updates a single worker document
        /// </summary>
        /// <param name="id">The ID of the worker to update</param>
        /// <param name="workerDto">The new worker data to use for the update</param>
        /// <returns>No content if successful otherwise a not found result if no worker was found</returns>
        [HttpPut]
        public async Task<ActionResult> PutWorker(string id, WorkerDto workerDto)
        {
            // Attempt to find the worker with the provided ID
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the worker exists, perform an update on their details, otherwise
            // return a not found result
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

        /// <summary>
        /// Deletes a single worker document
        /// </summary>
        /// <param name="id">The ID of the worker to be deleted</param>
        /// <returns>No content if successful, otherwise a not found result</returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteWorker(string id)
        {
            // Attempt to find the worker first before deletion
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the worker does exist, delete their data, otherwise
            // return a not found result
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
        /// Gets all the workers in a certain location
        /// </summary>
        /// <param name="locationId">The location to find workers at</param>
        /// <returns>A list of workers if any are found, otherwise a not found result if none exist</returns>
        [HttpGet("/api/[controller]/cities")]
        public async Task<ActionResult<List<Worker>>> GetWorkersInCity(string locationId)
        {
            // The list of workers to be returned
            List<Worker> workers = new List<Worker>();

            // Find all workers at the specified location
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("LocationId", locationId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // Convert each found document into a worker object and add it to the results list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Worker worker = documentSnapshot.ConvertTo<Worker>();

                worker.UserId = documentSnapshot.Id;

                workers.Add(worker);
            }

            // If no workers were found, then return a not found result
            if (workers.Count == 0)
            {
                return NotFound();
            }

            // Return the workers found at a specific location
            return Ok(workers);
        }

        // This will change to skills later, for now use the job IDs
        /// <summary>
        /// Gets all the workers who can perform certain jobs
        /// </summary>
        /// <param name="locationId">The location to search for</param>
        /// <param name="jobIds">The jobs to search for</param>
        /// <returns>A list of workers if any are found, otherwise a not found result</returns>
        [HttpGet("/api/[controller]/skills")]
        public async Task<ActionResult<List<Worker>>> GetWorkersWithSkills(string locationId, [FromQuery]List<string> jobIds)
        {
            // The list of worker results to return
            List<Worker> workers = new List<Worker>();

            // Query the database to find any workers in the specified location
            // and who have the right skills
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("LocationId", locationId)
                .WhereArrayContainsAny("JobIds", jobIds);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // Convert any found worker documents to a worker object and add it
            // to the results list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Worker worker = documentSnapshot.ConvertTo<Worker>();

                worker.UserId = documentSnapshot.Id;

                workers.Add(worker);
            }

            // If no documents were found, return a not found result
            if (workers.Count == 0)
            {
                return NotFound();
            }

            // Return any found worker objects
            return Ok(workers);
        }
    }
}
