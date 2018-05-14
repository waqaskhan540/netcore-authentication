using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Auth.LocalLogin.Services
{
    public interface IUserService
    {
        Task<bool> ValidateCredentials(string username, string password, out User user);
        Task<bool> AddUser(string username, string password);
    }
    public class User
    {
        public User(string username)
        {
            Username = username;
        }

        public string Username { get; }
    }
}
