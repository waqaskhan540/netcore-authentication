using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Custom.Auth.Web.Security;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CustomIdpExtensions
    {
        public static AuthenticationBuilder AddCustomIdp(this AuthenticationBuilder builder)
        {
            return builder.AddRemoteScheme<CustomIdpOptions, CustomIdpHandler>(CustomIdpDefaults.AuthenticationScheme, CustomIdpDefaults.DisplayName, x => { });
        }
        public static AuthenticationBuilder AddCustomIdp(this AuthenticationBuilder builder,  Action<CustomIdpOptions> options)
        {
            return builder.AddRemoteScheme<CustomIdpOptions, CustomIdpHandler>(CustomIdpDefaults.AuthenticationScheme, CustomIdpDefaults.DisplayName, options);
        }
        public static AuthenticationBuilder AddCustomIdp(this AuthenticationBuilder builder, string authenticationScheme,  Action<CustomIdpOptions> options)
        {
            return builder.AddRemoteScheme<CustomIdpOptions, CustomIdpHandler>(authenticationScheme, CustomIdpDefaults.DisplayName, options);
        }
        public static AuthenticationBuilder AddCustomIdp(this AuthenticationBuilder builder,string authenticationScheme, string displayName, Action<CustomIdpOptions> options)
        {
            return builder.AddRemoteScheme<CustomIdpOptions, CustomIdpHandler>(authenticationScheme, displayName, options);
        }
    }
}
