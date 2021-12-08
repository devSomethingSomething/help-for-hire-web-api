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
    /// This controller handles operations related to
    /// reports
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : ControllerBase
    {
        /// <summary>
        /// Reference to the collection in the Firestore database
        /// </summary>
        private const string COLLECTION = "Report";

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReportController()
        {

        }

        /// <summary>
        /// Creates a new report in the Firestore database
        /// </summary>
        /// <param name="reportDto">The new report to post</param>
        /// <returns>The created report otherwise a bad request result is returned</returns>
        [HttpPost]
        public async Task<ActionResult<Report>> PostReport(ReportDto reportDto)
        {
            // If the report is not valid, return a bad request
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Perform a query to check if the report is a duplicate
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("ReportedUserId", reportDto.ReportedUserId)
                .WhereEqualTo("ReporterUserId", reportDto.ReporterUserId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // If a duplicate report is found, return a bad request with the error
            if (querySnapshot.Documents.Count != 0)
            {
                return BadRequest("Duplicate report detected");
            }

            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            // Create the new report in the database
            await collectionReference.Document().SetAsync(reportDto);
            
            // Return the created report
            return CreatedAtAction(nameof(PostReport), reportDto);
        }

        /// <summary>
        /// Gets all the reports from the Firestore database
        /// </summary>
        /// <returns>A list of found reports if any exist, if none are found then a not found request is returned</returns>
        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Report>>> GetReports()
        {
            // Found reports to return
            List<Report> reports = new List<Report>();

            // Query the collection and get back all reports
            Query query = FirestoreManager.Db.Collection(COLLECTION);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // Each document gets converted to a report and added to the results list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Report report = documentSnapshot.ConvertTo<Report>();

                report.ReportId = documentSnapshot.Id;

                reports.Add(report);
            }

            // If no reports are found, then return a not found result
            if (reports.Count == 0)
            {
                return NotFound();
            }

            // Return reports if any were found
            return Ok(reports);
        }

        /// <summary>
        /// Deletes a single report from the database
        /// </summary>
        /// <param name="id">The ID of the report to delete</param>
        /// <returns>No content if successful otherwise not found if no report was found</returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteReport(string id)
        {
            // Attempt to reference a document with the provided ID
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // If the report document exists, delete it, otherwise if it is not found then
            // return not found
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
