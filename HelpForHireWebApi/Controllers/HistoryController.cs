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
    public class HistoryController : ControllerBase
    {
        private const string COLLECTION = "History";

        public HistoryController()
        {

        }

        [HttpPost]
        public async Task<ActionResult<History>> PostHistory(HistoryDto historyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            await collectionReference.Document().SetAsync(historyDto);

            return CreatedAtAction(nameof(PostHistory), historyDto);
        }

        [HttpGet]
        public async Task<ActionResult<History>> GetHistory(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if (documentSnapshot.Exists)
            {
                History history = documentSnapshot.ConvertTo<History>();

                history.HistoryId = id;

                return Ok(history);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<History>>> GetHistories()
        {
            List<History> histories = new List<History>();

            Query query = FirestoreManager.Db.Collection(COLLECTION);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                History history = documentSnapshot.ConvertTo<History>();

                history.HistoryId = documentSnapshot.Id;

                histories.Add(history);
            }

            if (histories.Count == 0)
            {
                return NotFound();
            }

            return Ok(histories);
        }

        [HttpPut]
        public async Task<ActionResult> PutHistory(string id, HistoryDto historyDto)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            if (documentReference == null)
            {
                return NotFound();
            }

            await documentReference.SetAsync(historyDto, SetOptions.MergeAll);

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteHistory(string id)
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

        [HttpGet("/api/[controller]/user")]
        public async Task<ActionResult<List<History>>> GetHistoryByUser(string userId)
        {
            List<History> histories = new List<History>();

            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("UserId", userId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                History history = documentSnapshot.ConvertTo<History>();

                history.HistoryId = documentSnapshot.Id;

                histories.Add(history);
            }

            if (histories.Count == 0)
            {
                return NotFound();
            }

            return Ok(histories);
        }
    }
}
