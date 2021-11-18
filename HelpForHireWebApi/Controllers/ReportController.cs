using Microsoft.AspNetCore.Mvc;
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
    public class ReportController : ControllerBase
    {
        private const string COLLECTION = "Report";

        public ReportController()
        {

        }

        [HttpPost]
        public async Task<ActionResult<Report>> PostReport(Report report)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CollectionReference collectionReference = FirestoreManager.Db
                    .Collection(COLLECTION);

            await collectionReference.Document(report.ReportId).SetAsync(report);

            return CreatedAtAction(nameof(PostReport), report);
        }

        [HttpGet]
        public async Task<ActionResult<Report>> GetReport(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if (documentSnapshot.Exists)
            {
                Report report = documentSnapshot.ConvertTo<Report>();

                report.ReportId = id;

                return Ok(report);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Report>>> GetReports()
        {
            List<Report> reports = new List<Report>();

            Query query = FirestoreManager.Db.Collection(COLLECTION);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Report report = documentSnapshot.ConvertTo<Report>();

                report.ReportId = documentSnapshot.Id;

                reports.Add(report);
            }

            if (reports.Count == 0)
            {
                return NotFound();
            }

            return Ok(reports);
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

    }
}
