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
    public class EmployerController : ControllerBase
    {
        private const string COLLECTION = "Employer";

        public EmployerController()
        {

        }

        [HttpPost]
        public async Task<ActionResult<Employer>> PostEmployer(Employer employer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            await collectionReference.Document(employer.UserId).SetAsync(employer);

            return CreatedAtAction(nameof(PostEmployer), employer);
        }

        [HttpGet]
        public async Task<ActionResult<Employer>> GetEmployer(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if (documentSnapshot.Exists)
            {
                Employer employer = documentSnapshot.ConvertTo<Employer>();

                employer.UserId = id;

                return Ok(employer);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Employer>>> GetEmployers()
        {
            List<Employer> employers = new List<Employer>();

            Query query = FirestoreManager.Db.Collection(COLLECTION);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Employer employer = documentSnapshot.ConvertTo<Employer>();

                employer.UserId = documentSnapshot.Id;

                employers.Add(employer);
            }

            if (employers.Count == 0)
            {
                return NotFound();
            }

            return Ok(employers);
        }

        [HttpPut]
        public async Task<ActionResult> PutEmployer(string id, EmployerDto employerDto)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            if (documentReference == null)
            {
                return NotFound();
            }

            await documentReference.SetAsync(employerDto, SetOptions.MergeAll);

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteEmployer(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            if (documentReference == null)
            {
                return NotFound();
            }

            await documentReference.DeleteAsync();

            return NoContent();
        }

        [HttpGet("/api/[controller]/cities")]
        public async Task<ActionResult<List<Employer>>> GetEmployersInCity(string locationId)
        {
            List<Employer> employers = new List<Employer>();

            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("LocationId", locationId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Employer employer = documentSnapshot.ConvertTo<Employer>();

                employer.UserId = documentSnapshot.Id;

                employers.Add(employer);
            }

            if (employers.Count == 0)
            {
                return NotFound();
            }

            return Ok(employers);
        }
    }
}
