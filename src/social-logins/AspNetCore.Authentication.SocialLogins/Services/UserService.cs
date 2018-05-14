using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.SocialLogins.Services
{
    public class UserService : IUserService
    {

        private IDictionary<string, User> _users = new Dictionary<string, User>();

        public Task<User> AddUser(string id, string displayname, string email)
        {
            var user = User.Create(id, displayname, email);
            _users.Add(user.Id, user);
            return Task.FromResult(user);
        }

        public Task<User> GetUserById(string Id)
        {
            if (_users.ContainsKey(Id))
            {
                return Task.FromResult(_users[Id]);
            }
            return Task.FromResult<User>(null);
        }
    }
}
