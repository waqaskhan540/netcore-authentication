using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Auth.LocalLogin.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IDictionary<string, UserProfile> _profiles;

        public ProfileService(IDictionary<string,UserProfile> profiles)
        {
            _profiles = profiles;
        }
        public Task<UserProfile> GetUserProfileAsync(string userId)
        {
            if (_profiles.ContainsKey(userId))
            {
                return Task.FromResult(_profiles[userId]);
            }

            return Task.FromResult<UserProfile>(null);
        }
    }
}
