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
    /// This controller handles all operations related
    /// to jobs
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        /// <summary>
        /// Reference to the collection in the Firestore database
        /// </summary>
        private const string COLLECTION = "Job";

        /// <summary>
        /// Default constructor
        /// </summary>
        public JobController()
        {

        }

        /// <summary>
        /// Creates a new job
        /// </summary>
        /// <param name="jobDto">The job data to post</param>
        /// <returns>The created job otherwise a bad request</returns>
        [HttpPost]
        public async Task<ActionResult<Job>> PostJob(JobDto jobDto)
        {
            // Check to see if the model is valid first
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Perform a query to check if the job already exists
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("Title", jobDto.Title);

            // Get a snapshot of the query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // If a duplicate is found, then return a bad request
            if (querySnapshot.Documents.Count != 0)
            {
                return BadRequest("Duplicate item detected");
            }

            // Get a reference to the collection
            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            // Create the new job
            await collectionReference.Document().SetAsync(jobDto);

            return CreatedAtAction(nameof(PostJob), jobDto);
        }

        /// <summary>
        /// Gets a specific job
        /// </summary>
        /// <param name="id">The ID of the job we want to retrieve</param>
        /// <returns>The found job otherwise a not found result</returns>
        [HttpGet]
        public async Task<ActionResult<Job>> GetJob(string id)
        {
            // Create a reference to the job we are looking for
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // Get a snapshot of the document
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the job exists, return it otherwise return a not found result
            if (documentSnapshot.Exists)
            {
                Job job = documentSnapshot.ConvertTo<Job>();

                job.JobId = id;

                return Ok(job);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets all the jobs in the database
        /// </summary>
        /// <returns>A list of jobs otherwise a not found result</returns>
        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Job>>> GetJobs()
        {
            // List of jobs to return
            List<Job> jobs = new List<Job>();

            // Perform a query to retrieve all the jobs in the collection
            Query query = FirestoreManager.Db.Collection(COLLECTION);

            // Get a snapshot of the query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // For every found job, add it to the jobs list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Job job = documentSnapshot.ConvertTo<Job>();

                job.JobId = documentSnapshot.Id;

                jobs.Add(job);
            }

            // If no jobs were found, return a not found result
            if (jobs.Count == 0)
            {
                return NotFound();
            }

            return Ok(jobs);
        }

        /// <summary>
        /// Get selected jobs for a user
        /// </summary>
        /// <param name="jobIds">The list of jobs we want to retrieve</param>
        /// <returns>A list of jobs otherwise a not found result</returns>
        [HttpGet("/api/[controller]/selected")]
        public async Task<ActionResult<List<Job>>> GetSelectedJobs(string[] jobIds)
        {
            // List of jobs to return
            List<Job> jobs = new List<Job>();

            // Get a list of jobs based off of the IDs provided to the method
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereArrayContainsAny("JobId", jobIds);

            // Get a snapshot of the query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // For any found jobs, add them to the results list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Job job = documentSnapshot.ConvertTo<Job>();

                job.JobId = documentSnapshot.Id;

                jobs.Add(job);
            }

            // If no jobs were found, return a not found result
            if (jobs.Count == 0)
            {
                return NotFound();
            }

            return Ok(jobs);
        }

        /// <summary>
        /// Updates a job
        /// </summary>
        /// <param name="id">The ID of the job we want to update</param>
        /// <param name="jobDto">The new job data to use in the update</param>
        /// <returns>No content if successful otherwise not found</returns>
        [HttpPut]
        public async Task<ActionResult> PutJob(string id, JobDto jobDto)
        {
            // Perform a query to search for duplicate jobs
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("Title", jobDto.Title);

            // Get a snapshot of this query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // If any duplicates were found, return a bad request
            if (querySnapshot.Documents.Count != 0)
            {
                return BadRequest("Duplicate item detected");
            }

            // Get a reference to the collection
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // Get a snapshot of this document
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the document exists, perform an update otherwise return a not found result
            if (documentSnapshot.Exists)
            {
                await documentReference.SetAsync(jobDto, SetOptions.MergeAll);

                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes a job
        /// </summary>
        /// <param name="id">The ID of the job we want to delete</param>
        /// <returns>No content if successful otherwise a not found result</returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteJob(string id)
        {
            // Get a reference to the document with the provided ID
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // Get a snapshot of this document
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the document exists, delete it otherwise return not found
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
    }
}
