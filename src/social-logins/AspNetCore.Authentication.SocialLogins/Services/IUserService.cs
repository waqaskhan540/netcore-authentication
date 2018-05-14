using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.SocialLogins.Services
{
    public interface IUserService
    {
        Task<User> GetUserById(string Id);
        Task<User> AddUser(string id, string displayname, string email);
    }
}
