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
        },
        new TestUser
        {
            SubjectId = "2",
            Username = "alice",
            Password = "password",
            Claims =
            {
                new Claim("name", "Alice Smith"),
                new Claim("email", "alice@example.com")
            }
        },
        new TestUser
        {
            SubjectId = "3",
            Username = "bob",
            Password = "password",
            Claims =
            {
                new Claim("name", "Bob Johnson"),
                new Claim("email", "bob@example.com")
            }
        }
    };
}