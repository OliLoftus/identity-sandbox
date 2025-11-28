using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Identity.Oli.Auth.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Oli.Auth.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;

    // Inject Duende's test user store and interaction service
    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        IIdentityServerInteractionService interaction)
    {
        _signInManager = signInManager;
        _interaction = interaction;
    }

    // POST /account/login
    // Handles login form POST and signs the user in if valid
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password, [FromForm] string returnUrl)
    {
        var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: true, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            // validate returnUrl is from IdentityServer to avoid open redirects
            if (_interaction.IsValidReturnUrl(returnUrl))
                return Redirect(returnUrl);

            return Redirect("~/"); // fallback
        }

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