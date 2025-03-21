using Identity.Oli.Models.Requests;
using Identity.Oli.Models.Responses;
using Identity.Oli.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Oli.Controllers;
[ApiController]
[Route("auth")]
public class LoginController : ControllerBase
{
    private readonly IJwtTokenService _jwtTokenService;

    public LoginController(IJwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Temporary hard-coded dev login check
        if (request.Username == "admin" && request.Password == "password")
        {
            var token = _jwtTokenService.GenerateToken(
                username: request.Username,
                scopes: new[] { "api1.read", "api1.write, api1.admin" }
                );

            return Ok(new ApiResponse<string>("Success", "Login successful", token));
        }

        return Unauthorized(new ApiResponse<string>("Failure", "Login failed", null));
    }

}