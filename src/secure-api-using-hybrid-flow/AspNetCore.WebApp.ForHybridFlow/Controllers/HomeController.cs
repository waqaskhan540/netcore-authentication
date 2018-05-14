using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.WebApp.ForHybridFlow.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;
using System.Net.Http;

namespace AspNetCore.WebApp.ForHybridFlow.Controllers
{
    public class HomeController : Controller
    {
        public Task DicoveryClient { get; private set; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }


        [Route("callapi")]
        [Authorize]
        public async Task<IActionResult> CallApi()
        {
            string accessToken;
            try
            {
                accessToken = await GetAccessToken();
            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.GetBaseException().Message;
                return View();
            }

            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            try
            {
                var content = await client.GetStringAsync("https://localhost:2991/api/user");
                ViewBag.ApiResponse = content;
            }
            catch (Exception ex)
            {

                ViewBag.ApiResponse = ex.GetBaseException().Message;
            }

            ViewBag.AccessToken = accessToken;
            ViewBag.RefreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            return View();
        }

        private async Task<string> GetAccessToken()
        {
            var exp = await HttpContext.GetTokenAsync("expires_at");
            var expires = DateTime.Parse(exp);
            if(expires > DateTime.Now)
            {
                return await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            }

            return await GetRefreshedAccessToken();
        }

        private async Task<string> GetRefreshedAccessToken()
        {
            var discoClient = await DiscoveryClient.GetAsync("https://localhost:44395");
            var tokenClient = new TokenClient(discoClient.TokenEndpoint, "Hybrid", "MySecret");
            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
            var tokenResponse = await tokenClient.RequestRefreshTokenAsync(refreshToken);

            if (!tokenResponse.IsError)
            {
                var auth = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                auth.Properties.UpdateTokenValue(OpenIdConnectParameterNames.AccessToken, tokenResponse.AccessToken);
                auth.Properties.UpdateTokenValue(OpenIdConnectParameterNames.RefreshToken, tokenResponse.RefreshToken);

                var expiry = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResponse.ExpiresIn);
                auth.Properties.UpdateTokenValue("expires_at", expiry.ToString("o", CultureInfo.InvariantCulture));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,auth.Principal,auth.Properties);
                return tokenResponse.AccessToken;

            }

            throw tokenResponse.Exception;
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        [Route("userinformation")]
        public IActionResult UserInfo()
        {
            var claims = User.Claims.Select(x => x.Value).ToList();
            return View(claims);
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
