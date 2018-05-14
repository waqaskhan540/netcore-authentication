using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.SocialLogins.Services
{
    public class User
    {

        private User()
        {

        }

        public static User Create(string Id,string displayname,string email)
        {
            return new User
            {
                Id = Id,
                DisplayName = displayname,
                Email = email
            };
        }
        public string Id { get; private set; }
        public string DisplayName { get; private set; }
        public string Email { get; private set; }
    }
}
