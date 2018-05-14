using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.ForHybridFlow.Controllers
{
    [Route("api")]
    public class ValuesController : Controller
    {

        [HttpGet("user")]
        [Authorize]
       public IActionResult UserInfo()
        {
            return Content($"User:{User.Identity.Name}");
        }
     
    }
}
