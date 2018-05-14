using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Custom.Auth.Web.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
      
        [Route("signin")]
        public IActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/"
            });
        }
    }
}
