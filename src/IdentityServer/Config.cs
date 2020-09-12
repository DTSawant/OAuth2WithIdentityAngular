// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
    new List<IdentityResource>
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
    };

        public static IEnumerable<ApiScope> ApiScopes =>
           new List<ApiScope> {
               new ApiScope("api1", "My Api"),
               new ApiScope("api2", "My Api 2")

               };

        public static IEnumerable<ApiResource> Apis =>
           new List<ApiResource> {
               new ApiResource("api1", "My Api"),
               new ApiResource("api2", "My Api 2")

               };

        public static IEnumerable<Client> Clients => new List<Client> {
            new Client
            {
                ClientId = "Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "api2"}
            },
             new Client
            {
                ClientId = "mvc",
                AllowedGrantTypes = GrantTypes.Code,
                // where to redirect to after login
                RedirectUris = { "http://localhost:5002/home/callback" },
                PostLogoutRedirectUris = { "http://localhost:5002/home/callback" },
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = {
                   IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                     "api1"
                 },
                AllowOfflineAccess = true,
                AccessTokenLifetime = 30
            },

              new Client
            {
                ClientId = "AngularClient",
                RequireClientSecret = false,
                AlwaysIncludeUserClaimsInIdToken = true,                
                AllowedGrantTypes = GrantTypes.Code,
                // where to redirect to after login
                RedirectUris = { "http://localhost:4200/LoginSuccess", "http://localhost:4500/LoginSuccess" },
                PostLogoutRedirectUris = { "http://localhost:4200/logoutsuccess", "http://localhost:4500/logoutsuccess" },
              
                AllowedScopes = {
                   IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                     "api1"
                 },
                AllowOfflineAccess = true,
                AccessTokenLifetime = 60,
                AllowedCorsOrigins = { "http://localhost:4200", "http://localhost:4300", "http://localhost:4500", "http://localhost:8081" }
            }
                
        };

    }
}