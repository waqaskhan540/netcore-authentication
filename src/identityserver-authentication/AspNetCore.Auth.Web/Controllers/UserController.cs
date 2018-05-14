using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Auth.Web.Controllers
{
    public class UserController : Controller
    {
        [Authorize]
        [HttpGet("userinformation")]
        public IActionResult UserInfo()
        {
            var claims = User.Claims.Select(x => x.Value).ToList();
            return View(claims);
        }
    }
}
