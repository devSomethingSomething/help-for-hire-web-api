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
    /// This controller handles operations related
    /// to employers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmployerController : ControllerBase
    {
        /// <summary>
        /// Reference to the collection in the Firestore database
        /// </summary>
        private const string COLLECTION = "Employer";

        /// <summary>
        /// Default constructor
        /// </summary>
        public EmployerController()
        {

        }

        /// <summary>
        /// Post method for creating new employers
        /// </summary>
        /// <param name="employer">The new employer to create</param>
        /// <returns>An employer if successful otherwise a bad request</returns>
        [HttpPost]
        public async Task<ActionResult<Employer>> PostEmployer(Employer employer)
        {
            // First check if the model is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create reference to the employer collection
            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            // Create new employer
            await collectionReference.Document(employer.UserId).SetAsync(employer);

            // Return created employer
            return CreatedAtAction(nameof(PostEmployer), employer);
        }

        /// <summary>
        /// Gets back a specific employer with the specified ID
        /// </summary>
        /// <param name="id">The ID of the employer we want to retrieve</param>
        /// <returns>An employer if found otherwise a not found result</returns>
        [HttpGet]
        public async Task<ActionResult<Employer>> GetEmployer(string id)
        {
            // Create a document reference to a specific employer with the ID
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // Get a snapshot of this document
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            // Check if the employer does exist
            if (documentSnapshot.Exists)
            {
                Employer employer = documentSnapshot.ConvertTo<Employer>();

                employer.UserId = id;

                // Return found employer
                return Ok(employer);
            }
            else
            {
                // Return not found if the employer does not exist
                return NotFound();
            }
        }

        /// <summary>
        /// Gets back all the employers in the Firestore database
        /// </summary>
        /// <returns>A list of employers if any were found otherwise a not found result</returns>
        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Employer>>> GetEmployers()
        {
            // Create a new list of employers for storing any retrieved results
            List<Employer> employers = new List<Employer>();

            // Create a query to the employer collection
            Query query = FirestoreManager.Db.Collection(COLLECTION);

            // Get a snapshot of this query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // For each found document, create a new employer and add them to the employer list
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Employer employer = documentSnapshot.ConvertTo<Employer>();

                employer.UserId = documentSnapshot.Id;

                employers.Add(employer);
            }

            // Return not found if the list of results is empty
            if (employers.Count == 0)
            {
                return NotFound();
            }

            // Otherwise return the found employers, this could be one or more
            return Ok(employers);
        }

        /// <summary>
        /// Updates a specific employer
        /// </summary>
        /// <param name="id">The ID of the employer to update</param>
        /// <param name="employerDto">The new employer details to use in the update</param>
        /// <returns>No content if successful otherwise not found</returns>
        [HttpPut]
        public async Task<ActionResult> PutEmployer(string id, EmployerDto employerDto)
        {
            // Create a reference to a document with the specified ID
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // If the document does not exist return not found
            if (documentReference == null)
            {
                return NotFound();
            }

            // Otherwise perform the update on the employer
            await documentReference.SetAsync(employerDto, SetOptions.MergeAll);

            return NoContent();
        }

        /// <summary>
        /// Deletes an employer from the Firestore database
        /// </summary>
        /// <param name="id">ID of the employer we want to delete</param>
        /// <returns>No content if successful otherwise not found</returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteEmployer(string id)
        {
            // Create a reference to an employer document with the specified ID
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            // If the document is null return not found
            if (documentReference == null)
            {
                return NotFound();
            }

            // Otherwise delete the employer document
            await documentReference.DeleteAsync();

            return NoContent();
        }

        /// <summary>
        /// Finds all the employers in a specific city
        /// </summary>
        /// <param name="locationId">The ID of the location we want to search for</param>
        /// <returns>A list of employers if any are found otherwise a not found result</returns>
        [HttpGet("/api/[controller]/cities")]
        public async Task<ActionResult<List<Employer>>> GetEmployersInCity(string locationId)
        {
            // Employer list for any results
            List<Employer> employers = new List<Employer>();

            // Perform a query to find any documents where the location ID matches the one
            // provided to the method
            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("LocationId", locationId);

            // Get a snapshot of this query
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            // For any found results, create an employer object and add them
            // to the list of employer results
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Employer employer = documentSnapshot.ConvertTo<Employer>();

                employer.UserId = documentSnapshot.Id;

                employers.Add(employer);
            }

            // Return not found if there is no employers in the list
            if (employers.Count == 0)
            {
                return NotFound();
            }

            return Ok(employers);
        }
    }
}
