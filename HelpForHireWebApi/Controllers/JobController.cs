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
    public class JobController : ControllerBase
    {
        private const string COLLECTION = "Job";

        public JobController()
        {

        }

        [HttpPost]
        public async Task<ActionResult<Job>> PostJob(JobDto jobDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("Title", jobDto.Title);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            if (querySnapshot.Documents.Count != 0)
            {
                return BadRequest("Duplicate item detected");
            }

            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            await collectionReference.Document().SetAsync(jobDto);

            return CreatedAtAction(nameof(PostJob), jobDto);
        }

        [HttpGet]
        public async Task<ActionResult<Job>> GetJob(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

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

        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Job>>> GetJobs()
        {
            List<Job> jobs = new List<Job>();

            Query query = FirestoreManager.Db.Collection(COLLECTION);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Job job = documentSnapshot.ConvertTo<Job>();

                job.JobId = documentSnapshot.Id;

                jobs.Add(job);
            }

            if (jobs.Count == 0)
            {
                return NotFound();
            }

            return Ok(jobs);
        }

        [HttpGet("/api/[controller]/selected")]
        public async Task<ActionResult<List<Job>>> GetSelectedJobs(String[] ids)
        {
            List<Job> jobs = new List<Job>();

            Query query = FirestoreManager.Db.Collection(COLLECTION).WhereArrayContainsAny("JobId",ids);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Job job = documentSnapshot.ConvertTo<Job>();

                job.JobId = documentSnapshot.Id;

                jobs.Add(job);
            }

            if (jobs.Count == 0)
            {
                return NotFound();
            }

            return Ok(jobs);
        }

        [HttpPut]
        public async Task<ActionResult> PutJob(string id, JobDto jobDto)
        {
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("Title", jobDto.Title);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            if (querySnapshot.Documents.Count != 0)
            {
                return BadRequest("Duplicate item detected");
            }

            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

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

        [HttpDelete]
        public async Task<ActionResult> DeleteJob(string id)
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
    }
}
