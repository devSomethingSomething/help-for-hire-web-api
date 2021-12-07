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
    /// <summary>
    /// This controller handles operations related
    /// to authentication
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public AuthController()
        {

        }

        /// <summary>
        /// Put method for updating users authentication information.
        /// 
        /// Used for updating their passwords
        /// </summary>
        /// <param name="email">The users ID as an email. Example, "1234567890123@helpforhire.com"</param>
        /// <param name="password">The users new password</param>
        /// <returns>No content if the operation was successful</returns>
        [HttpPut]
        public async Task<ActionResult> PutAuth(string email, string password)
        {
            // Get the record of the user using their email
            UserRecord userRecord = await FirestoreManager.Auth.GetUserByEmailAsync(email);

            // Create new user args for updating their password
            UserRecordArgs userRecordArgs = new UserRecordArgs()
            {
                // Required property otherwise Firebase does not know which user
                // needs to be updated
                Uid = userRecord.Uid,
                Email = email,
                Password = password,
            };

            // Update the users authentication information
            await FirestoreManager.Auth.UpdateUserAsync(userRecordArgs);

            return NoContent();
        }

        /// <summary>
        /// Delete method for removing a user from the Firebase authentication
        /// system
        /// </summary>
        /// <param name="email">The users ID as an email. Example, "1234567890123@helpforhire.com"</param>
        /// <returns>No content if the operation was successful</returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteAuth(string email)
        {
            // Get the record of the user using their email
            UserRecord userRecord = await FirestoreManager.Auth.GetUserByEmailAsync(email);

            // Delete the user from Firebase authentication
            await FirebaseAuth.DefaultInstance.DeleteUserAsync(userRecord.Uid);

            return NoContent();
        }
    }
}
