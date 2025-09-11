using System.Security.Cryptography;
using System.Text;

namespace Identity.Oli.Auth.Utils;

public class PkceHelper
{
    public static (string CodeVerifier, string CodeChallenge) GeneratePkceValues()
    {
        // Generate 32 random bytes
        var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32];
        rng.GetBytes(bytes);

        // Base64 URL-encode for verifier
        var codeVerifier = Convert.ToBase64String(bytes)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");

        // SHA256 and base64 URL-encode for challenge
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
        var codeChallenge = Convert.ToBase64String(hash)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");

        return (codeVerifier, codeChallenge);
    }
}