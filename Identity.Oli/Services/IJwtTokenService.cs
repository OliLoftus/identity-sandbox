namespace Identity.Oli.Services;

public interface IJwtTokenService
{
    string GenerateToken(string username, string[] scopes);
}