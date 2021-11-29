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
        public async Task<ActionResult<Report>> PostReport(ReportDto reportDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("ReportedUserId", reportDto.ReportedUserId)
                .WhereEqualTo("ReporterUserId", reportDto.ReporterUserId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            if (querySnapshot.Documents.Count != 0)
            {
                return BadRequest("Duplicate report detected");
            }

            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            await collectionReference.Document().SetAsync(reportDto);

            return CreatedAtAction(nameof(PostReport), reportDto);
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

        [HttpDelete]
        public async Task<ActionResult> DeleteReport(string id)
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
