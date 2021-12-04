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
    public class InviteController : ControllerBase
    {
        private const string COLLECTION = "Invite";

        public InviteController()
        {

        }

        [HttpPost]
        public async Task<ActionResult<Invite>> PostInvite(InviteDto inviteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("EmployerId", inviteDto.EmployerId)
                .WhereEqualTo("WorkerId", inviteDto.WorkerId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            if (querySnapshot.Documents.Count != 0)
            {
                return BadRequest("Duplicate invite detected");
            }

            CollectionReference collectionReference = FirestoreManager.Db
                .Collection(COLLECTION);

            await collectionReference.Document().SetAsync(inviteDto);

            return CreatedAtAction(nameof(PostInvite), inviteDto);
        }

        [HttpGet]
        public async Task<ActionResult<Invite>> GetInvite(string id)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if (documentSnapshot.Exists)
            {
                Invite invite = documentSnapshot.ConvertTo<Invite>();

                invite.InviteId = id;

                return Ok(invite);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("/api/[controller]/all")]
        public async Task<ActionResult<List<Invite>>> GetInvites()
        {
            List<Invite> invites = new List<Invite>();

            Query query = FirestoreManager.Db.Collection(COLLECTION);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Invite invite = documentSnapshot.ConvertTo<Invite>();

                invite.InviteId = documentSnapshot.Id;

                invites.Add(invite);
            }

            if (invites.Count == 0)
            {
                return NotFound();
            }

            return Ok(invites);
        }

        [HttpPut]
        public async Task<ActionResult> PutInvite(string id, InviteDto inviteDto)
        {
            DocumentReference documentReference = FirestoreManager.Db
                .Collection(COLLECTION).Document(id);

            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if (documentSnapshot.Exists)
            {
                await documentReference.SetAsync(inviteDto, SetOptions.MergeAll);

                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteInvite(string id)
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

        [HttpGet("/api/[controller]/employer")]
        public async Task<ActionResult<List<Invite>>> GetInvitesForEmployer(string employerId)
        {
            List<Invite> invites = new List<Invite>();

            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("EmployerId", employerId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Invite invite = documentSnapshot.ConvertTo<Invite>();

                invite.InviteId = documentSnapshot.Id;

                invites.Add(invite);
            }

            if (invites.Count == 0)
            {
                return NotFound();
            }

            return Ok(invites);
        }

        [HttpGet("/api/[controller]/worker")]
        public async Task<ActionResult<List<Invite>>> GetInvitesForWorker(string workerId)
        {
            List<Invite> invites = new List<Invite>();

            Query query = FirestoreManager.Db.Collection(COLLECTION)
                .WhereEqualTo("WorkerId", workerId);

            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();

            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Invite invite = documentSnapshot.ConvertTo<Invite>();

                invite.InviteId = documentSnapshot.Id;

                invites.Add(invite);
            }

            if (invites.Count == 0)
            {
                return NotFound();
            }

            return Ok(invites);
        }
    }
}
