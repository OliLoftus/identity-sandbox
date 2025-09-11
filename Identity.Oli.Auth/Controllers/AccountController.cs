using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Duende.IdentityServer;

namespace Identity.Oli.Auth.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly TestUserStore _users;
    private readonly IIdentityServerInteractionService _interaction;

    // Inject Duende's test user store and interaction service
    public AccountController(TestUserStore users, IIdentityServerInteractionService interaction)
    {
        _users = users;
        _interaction = interaction;
    }

    // POST /account/login
    // Handles login form POST and signs the user in if valid
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromForm] string username,
        [FromForm] string password,
        [FromForm] string returnUrl)
    {
        Console.WriteLine($"POST login hit for user: {username}");

        // Validate the username/password using the test user store
        if (_users.ValidateCredentials(username, password))
        {
            var user = _users.FindByUsername(username);

            // Create an IdentityServerUser (Duende abstraction of a logged-in user)
            var isUser = new IdentityServerUser(user.SubjectId)
            {
                DisplayName = user.Username
            };
            var props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60) // optional
            };

            // Sign the user in — this sets the IdentityServer cookie
            await HttpContext.SignInAsync(isUser, props);

            // Redirect back to the original returnUrl from /connect/authorize
            return Redirect(returnUrl);
        }

        // If login failed, return a 401
        return Unauthorized("Invalid login.");
    }

    // GET /account/login
    // Handles the initial redirect from /connect/authorize
    [HttpGet("login")]
    public IActionResult Login([FromQuery] string returnUrl)
    {
        var vm = new LoginViewModel { ReturnUrl = returnUrl };
        return View(vm);
    }

    [HttpGet("/home/error")]
    public IActionResult Error(string errorId)
    {
        return Content($"An error occurred during the authorization flow. Error ID: {errorId}");
    }
}