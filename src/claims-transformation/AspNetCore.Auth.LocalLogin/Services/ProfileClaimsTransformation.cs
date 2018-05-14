

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace AspNetCore.Auth.LocalLogin.Services
{
    public class ProfileClaimsTransformation : IClaimsTransformation
    {
        private readonly IProfileService _profileService;

        public ProfileClaimsTransformation(IProfileService profileService)
        {
            _profileService = profileService;
        }
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identities.FirstOrDefault(x => x.IsAuthenticated);
            if (identity == null)
            {
                return principal;
            }
            var idClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
            var profile = await _profileService.GetUserProfileAsync(idClaim.Value);

            if (profile == null)
            {
                return principal;
            }

            var claims = new List<Claim>
            {
                idClaim,
                new Claim(ClaimTypes.GivenName,profile.Firstname,ClaimValueTypes.String,"ProfileClaimsTransformation"),
                new Claim(ClaimTypes.Surname,profile.Lastname,ClaimValueTypes.String,"ProfileClaimsTransformation"),
                new Claim(ClaimTypes.Name,profile.Firstname + " " + profile.Lastname,ClaimValueTypes.String,"ProfileClaimsTransformation"),

            };

            claims.AddRange(profile.Roles.Select(x => new Claim(ClaimTypes.Role, x, ClaimValueTypes.String, "ProfileClaimsTransformation")));

            var newIdentity = new ClaimsIdentity(claims, identity.AuthenticationType);
            return new ClaimsPrincipal(newIdentity);
        }
    }
}
