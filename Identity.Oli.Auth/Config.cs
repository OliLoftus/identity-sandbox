using Duende.IdentityServer.Models;

namespace Identity.Oli.Auth;

public static class Config
{
    public static IEnumerable<Client> Clients =>
        new[]
        {   // Machine to machine
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "api1.read", "api1.write" }
            },

            // PKCE user-based login
            new Client
            {
                ClientId = "spa-client",
                ClientName = "SPA Client",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,

                RedirectUris = { "http://localhost:3000/callback" },
                PostLogoutRedirectUris = { "http://localhost:3000" },
                AllowedCorsOrigins = { "http://localhost:3000" },

                AllowedScopes = { "openid", "profile", "api1.read" },
                AllowAccessTokensViaBrowser = true
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new[]
        {
            new ApiScope("api1.read", "Read access to API 1"),
            new ApiScope("api1.write", "Write access to API 1")
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new[]
        {
            new ApiResource("api1", "My Protected API")
            {
                Scopes = { "api1.read", "api1.write" }
            }
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
         new IdentityResource[]
         {
             new IdentityResources.OpenId(),
             new IdentityResources.Profile()
         };
 }