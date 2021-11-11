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
    public class JobController : ControllerBase
    {
        public JobController()
        {

        }

        [HttpGet]
        public async Task<ActionResult<Job>> GetJob(int id)
        {
            DocumentReference documentReference = FirestoreManager.Db.Collection("Job").Document(id.ToString());

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if (documentSnapshot.Exists)
            {
                Job job = documentSnapshot.ConvertTo<Job>();

                job.Id = id;

                return Ok(job);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostJob(Job job)
        {
            DocumentReference documentReference = FirestoreManager.Db.Collection("Job").Document((job.Id).ToString());

            await documentReference.SetAsync(job);

            return CreatedAtAction(nameof(PostJob), job);
        }

        [HttpPut]
        public async Task<ActionResult> PutJob(int id, Job job)
        {
           
            if (id != job.Id)
            {
                return BadRequest();
            }

            DocumentReference documentReference = FirestoreManager.Db.Collection("Job").Document(id.ToString());
         
            if (documentReference is null)
            {        
                return NotFound();
            }

            await documentReference.SetAsync(job);

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteJob(int id)
        {
            DocumentReference documentReference = FirestoreManager.Db.Collection("Job").Document(id.ToString());

            if (documentReference == null)
            {
                return NotFound();
            }

            await documentReference.DeleteAsync();
            return NoContent();
        }
    }
}