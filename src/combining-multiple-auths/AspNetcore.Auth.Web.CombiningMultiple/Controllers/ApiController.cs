using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetcore.Auth.Web.CombiningMultiple.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {

        [HttpGet("userinformation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetUserInformation()
        {
            return Ok(new
            {
                Id = User.FindFirst("sub").Value,
                Username = User.Identity.Name
            });
        }
    }
}
