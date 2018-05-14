using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNetCore.Auth.Encryption;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace AspNetCore.Custom.Auth.Web.Security
{
    public class CustomIdpHandler : RemoteAuthenticationHandler<CustomIdpOptions>
    {
        public CustomIdpHandler(IOptionsMonitor<CustomIdpOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (string.IsNullOrWhiteSpace(properties.RedirectUri))
            {
                properties.RedirectUri = CurrentUri;
            }

            var callbackUri = BuildRedirectUri(Options.CallbackPath + "?redirectUri=" + properties.RedirectUri);
            Context.Response.Redirect(Options.IdpUri + "/SignIn?returnUrl=" + UrlEncoder.Encode(callbackUri));
            return Task.CompletedTask;

        }
        protected override Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            if (!Request.Form.ContainsKey("payload"))
            {
                return Task.FromResult(HandleRequestResult.Fail("the response didn't contain payload."));
            }

            var payload = Request.Form["payload"];
            var redirectUri = "/";
            if (Request.Query.ContainsKey("redirectUri"))
            {
                redirectUri = Request.Query["redirectUri"];
            }

            var json = EncryptionHelper.Decrypt(payload, Options.DecryptionKey);
            var user = JObject.Parse(json);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Value<string>("username"),ClaimValueTypes.String,ClaimsIssuer),
                new Claim(ClaimTypes.Name,user.Value<string>("username"),ClaimValueTypes.String,ClaimsIssuer),
            };

            var roles = (JArray)user["roles"];
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x.Value<string>(), ClaimValueTypes.String, ClaimsIssuer)));

            var identity = new ClaimsIdentity(claims, ClaimsIssuer);
            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties { RedirectUri = redirectUri }, Scheme.Name);
            return Task.FromResult(HandleRequestResult.Success(ticket));

        }
    }
}
