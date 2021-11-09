using Google.Cloud.Firestore;
using HelpForHireWebApi.Managers;
using HelpForHireWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpForHireWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoryController : Controller
    {
       public HistoryController()
        {

        }

        [HttpGet]
        public async Task<ActionResult<History>> GetHistory(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db.Collection("History").Document(id);
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if(documentSnapshot.Exists)
            {
                History history = documentSnapshot.ConvertTo<History>();
                history.Id = id;
                return Ok(history);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<History>> PostHistory(History history)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CollectionReference collectionReference = FirestoreManager.Db.Collection("History");

            await collectionReference.Document().SetAsync(history);

            return CreatedAtAction(nameof(PostHistory), history);
        }

        [HttpPut]
        public async Task<ActionResult> PutHistory(string id, History history)
        {
            DocumentReference documentReference = FirestoreManager.Db.Collection("History").Document(id);

            if(documentReference == null)
            {
                return NotFound();
            }

            await documentReference.SetAsync(history, SetOptions.MergeAll);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteHistory(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db.Collection("History").Document(id);

            if (documentReference == null)
            {
                return NotFound();
            }

            await documentReference.DeleteAsync();
            return NoContent();
        }
    }
}
