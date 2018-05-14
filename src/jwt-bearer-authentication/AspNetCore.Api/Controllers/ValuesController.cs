using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.Controllers
{
    [Route("api")]
    [Produces("application/json")]
    [Authorize]
    public class ValuesController : Controller
    {
        [HttpGet("text/welcome")]
        public IActionResult GetWelcomeText()
        {
            return Content($"Welcome {User.Identity.Name}");
        }

        [HttpGet("user")]
        public IActionResult GetUserInfo()
        {
            return Content($"User:{User.Identity.Name}");
        }
    }
}
