using Duende.IdentityServer.Models;

namespace Identity.Oli.QuickStart;

public static class Config
{
    // Define API scopes
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("api1.read", "Read access to My API"),
            new ApiScope("api1.write", "Write access to My API"),
            new ApiScope("api1.admin", "Admin access to My API")
        };

    // Define API resources
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource("api1", "My API")
            {
                Scopes = { "api1.read", "api1.write", "api1.admin"}
            }
        };

    // Define clients
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "api1.read", "api1.write", "api1.admin" }
            }
        };
}