using Duende.IdentityServer;
using Duende.IdentityServer.Test;
using Identity.Oli.Auth;
using Identity.Oli.Auth.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This registers all services required for MVC and Razor Views.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddTestUsers(TestUsers.Users)
    .AddDeveloperSigningCredential();

// This configures the IdentityServer cookie
builder.Services.Configure<CookieAuthenticationOptions>(IdentityServerConstants.DefaultCookieAuthenticationScheme, options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
});

// This is for the test user store, which is fine for this project
builder.Services.AddSingleton(new TestUserStore(TestUsers.Users));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

// This is the key part for making controller views work correctly.
// It sets up the default route: /{controller=Home}/{action=Index}/{id?}
app.MapDefaultControllerRoute();

app.Run();

void CheckSameSite(HttpContext httpContext, CookieOptions options)
{
    if (options.SameSite == SameSiteMode.None)
    {
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        if (MyUserAgentDetectionLib.IsOldBrowser(userAgent)) // Your custom logic
        {
            options.SameSite = SameSiteMode.Unspecified;
        }
    }
}

// Dummy class for the purpose of the example
public static class MyUserAgentDetectionLib
{
    public static bool IsOldBrowser(string userAgent) => false;
}