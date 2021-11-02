using HelpForHireWebApi.Models;
using HelpForHireWebApi.Services;
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

        [HttpGet]
        public ActionResult<Auth> GetAuth(string id)
        {
            Auth auth = AuthService.GetAuth(id);

            if (auth == null)
            {
                return NotFound();
            }

            return auth;
        }
    }
}
