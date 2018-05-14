using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace AspNetCore.Auth.IdentityServer
{
    public static class IdentityServerConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }


        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("DemoApi"),
                new ApiResource("WebApi",new[] {"name" })
            };
        }
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "AuthWeb",
                    ClientName = "Auth WEb demo cleint",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris =
                    {
                        "https://localhost:44328/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string> {"https://localhost:44328/signout-callback-oidc"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                 new Client
                {
                    ClientId = "CombinedWeb",
                    ClientName = "Auth WEb demo cleint",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    RedirectUris =
                    {
                        "https://localhost:44314/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string> {"https://localhost:44314/signout-callback-oidc"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                },
                 new Client
                 {
                     ClientId = "AuthWeb_Javascript",
                     AllowedGrantTypes = GrantTypes.Implicit,
                     RedirectUris = { "https://localhost:44314/SilentSignInCallback.html" },
                     AllowedScopes = new List<string>
                     {
                         IdentityServerConstants.StandardScopes.OpenId,
                         "WebApi"
                     },
                     AllowedCorsOrigins = {"https://localhost:44314"},
                     AllowAccessTokensViaBrowser = true,
                     RequireConsent = false                     
                 },
                new Client
                {
                    ClientId = "WebApp",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new [] {new Secret("MySecret".Sha256()) },
                    AllowedScopes = new List<string> {"DemoApi"}
                },
                new Client
                {
                    ClientId = "Spa",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        "DemoApi"
                    },
                    RedirectUris =
                    {
                        "https://localhost:44377/SignInCallback.html"
                    },
                    PostLogoutRedirectUris = {"https://localhost:44377/SignoutCallback.html"},
                    AllowedCorsOrigins = {"https://localhost:44377"},
                    RequireConsent = false
                },
                new Client
                {
                   ClientId = "Hybrid",
                   ClientSecrets = new [] {new Secret("MySecret".Sha256())},
                   AllowedGrantTypes = GrantTypes.Hybrid,
                   AllowedScopes = new List<string>
                   {
                       IdentityServerConstants.StandardScopes.OpenId,
                       IdentityServerConstants.StandardScopes.Profile,
                       "DemoApi"
                   },
                   RedirectUris = {
                        "https://localhost:44360/signin-oidc"
                    },
                   PostLogoutRedirectUris = new List<string>
                   {
                       "https://localhost:44360/signout-callback-oidc"
                   },
                   AllowOfflineAccess = true,
                   RequireConsent = false,
                   AccessTokenLifetime = 5
                }
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "waqas",
                    Password = "test",
                    Claims = new []
                    {
                        new Claim("name","waqas")

                    }
                }
            };
        }
    }
}
