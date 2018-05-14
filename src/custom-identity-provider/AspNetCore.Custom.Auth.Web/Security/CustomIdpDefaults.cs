using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Custom.Auth.Web.Security
{
    public static class CustomIdpDefaults
    {
        public static readonly string AuthenticationScheme = "CustomIdp";
        public static readonly string DisplayName = "Custom Identity Provider";
    }
}
