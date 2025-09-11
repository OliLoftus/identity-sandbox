using Duende.IdentityServer.Test;
using System.Security.Claims;

namespace Identity.Oli.Auth.Utils;

public static class TestUsers
{
    public static List<TestUser> Users => new()
    {
        new TestUser
        {
            SubjectId = "1",
            Username = "oli",
            Password = "password",
            Claims =
            {
                new Claim("name", "Oli Loftus"),
                new Claim("email", "oli@example.com")
            }
        }
    };
}