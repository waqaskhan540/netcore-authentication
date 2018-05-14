using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Auth.LocalLogin.Services
{
    public interface IProfileService
    {
        Task<UserProfile> GetUserProfileAsync(string userId);
    }

    public class UserProfile
    {
        public UserProfile(string firstname,string lastname,string[] roles)
        {
            Firstname = firstname;
            Lastname = lastname;
            Roles = roles;
        }

        public string Firstname { get; }
        public string Lastname { get; }
        public string[] Roles { get; }
    }
}
