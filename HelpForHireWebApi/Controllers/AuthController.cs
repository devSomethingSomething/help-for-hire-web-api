using FirebaseAdmin.Auth;
using HelpForHireWebApi.Managers;
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
    public class AuthController : ControllerBase
    {
        public AuthController()
        {

        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAuth(string email)
        {
            UserRecord userRecord = await FirestoreManager.Auth.GetUserByEmailAsync(email);

            await FirebaseAuth.DefaultInstance.DeleteUserAsync(userRecord.Uid);

            return NoContent();
        }
    }
}
