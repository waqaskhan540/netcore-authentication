using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace AspNetCore.Custom.Auth.Web.Security
{
    public class CustomIdpOptions : RemoteAuthenticationOptions
    {
        public CustomIdpOptions()
        {
            CallbackPath = "/signin-dummyidp";
        }
        public string IdpUri { get; set; }
        public string DecryptionKey { get; set; }
    }
}
