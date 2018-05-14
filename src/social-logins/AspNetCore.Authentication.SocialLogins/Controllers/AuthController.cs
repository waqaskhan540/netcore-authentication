using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Authentication.SocialLogins.Models;
using AspNetCore.Authentication.SocialLogins.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Authentication.SocialLogins.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [Route("signin")]
        public async  Task<IActionResult> SignIn()
        {
            var authResult = await HttpContext.AuthenticateAsync("Temporary");
            if (authResult.Succeeded)
            {
                return RedirectToAction("Profile");
            }
            return View();
        }
        [Route("signin/{provider}")]
        public IActionResult SignIn(string provider, string returnUrl = null)
        {

            var redirectUri = Url.Action("Profile");
            if(returnUrl != null)
            {
                redirectUri += "?returnUrl=" + returnUrl;
            }
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = redirectUri
            }, provider);
        }
        [Route("signout")]
        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Route("signin/profile")]
        public async Task<IActionResult> Profile(string returnUrl = null)
        {
            var authResult = await HttpContext.AuthenticateAsync("Temporary");
            if (!authResult.Succeeded)
            {
                return RedirectToAction("SignIn");
            }

            var user = await _userService.GetUserById(authResult.Principal.FindFirst(ClaimTypes.NameIdentifier).Value);
            if(user != null)
            {
                return await SignInUser(user, returnUrl);
            }
            var model = new ProfileModel
            {
                DisplayName = authResult.Principal.Identity.Name
            };

            var emailClaim = authResult.Principal.FindFirst(ClaimTypes.Email);
            if(emailClaim != null)
            {
                model.Email = emailClaim.Value;
            }

            return View(model);
        }

        [Route("signin/profile")]
        [HttpPost]
        public async Task<IActionResult> Profile(ProfileModel model,string returnUrl)
        {
            var authResult = await HttpContext.AuthenticateAsync("Temporary");
            if (!authResult.Succeeded)
            {
                return RedirectToAction("SignIn");
            }

            if (ModelState.IsValid)
            {
                var user = await _userService.AddUser(authResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier), model.DisplayName, model.Email);
                return await SignInUser(user, returnUrl);
            }

            return View(model);
        }

        private async Task<IActionResult> SignInUser(User user, string returnUrl)
        {
            await HttpContext.SignOutAsync("Temporary");
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.DisplayName),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Redirect(returnUrl == null ? "/" : returnUrl);
        }
    }
}
