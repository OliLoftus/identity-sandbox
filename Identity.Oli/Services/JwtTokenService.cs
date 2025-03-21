using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Oli.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _config;

    public JwtTokenService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(string username, string[] scopes)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim("sub", username),
            new Claim("scope", string.Join(" ", scopes))
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}