using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Jwt.Bearer.Auth.Controllers
{
   
   
    
    public class HomeController : Controller
    {
      
      
       public async Task<IActionResult> Index()
        {
            //points to the identityserver uri..

            var disco = await DiscoveryClient.GetAsync("https://localhost:44395/");
            var tokenClient = new TokenClient(disco.TokenEndpoint, "WebApp", "MySecret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("DemoApi");

            object model;
            if (tokenResponse.IsError)
            {
                model = "error..could not get token";
            }else
            {
                var client = new HttpClient();
                client.SetBearerToken(tokenResponse.AccessToken);
                var response = await client.GetAsync("https://localhost:44375/api/text/welcome");
                if (response.IsSuccessStatusCode)
                {
                    model = await response.Content.ReadAsStringAsync();
                }else
                {
                    model = "Error..could not retrieve text.";
                }
            }

            return View(model);
        }
    }
}
