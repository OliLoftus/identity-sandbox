using Duende.IdentityServer;
using Identity.Oli.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.Oli.Auth.Data;
using Identity.Oli.Auth.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// This registers all services required for MVC and Razor Views.
builder.Services.AddControllersWithViews();

// ASP.NET Identity + SQLite for user store
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddAspNetIdentity<ApplicationUser>()
    .AddDeveloperSigningCredential(persistKey: false);

// This configures the IdentityServer cookie
builder.Services.Configure<CookieAuthenticationOptions>(IdentityServerConstants.DefaultCookieAuthenticationScheme, options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

// Enable ASP.NET Core Identity cookies
app.UseAuthentication();

app.UseIdentityServer();
app.UseAuthorization();

// This is the key part for making controller views work correctly.
// It sets up the default route: /{controller=Home}/{action=Index}/{id?}
app.MapDefaultControllerRoute();

// Ensure database exists and seed a few users for dev
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Seed an Admin role
    const string adminRole = "Admin";
    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    async Task EnsureUser(string userName, string email, bool isAdmin)
    {
        var user = await userManager.FindByNameAsync(userName);
        if (user == null)
        {
            user = new ApplicationUser { UserName = userName, Email = email, EmailConfirmed = true };
            var createResult = await userManager.CreateAsync(user, "Passw0rd!");
            if (!createResult.Succeeded)
            {
                Console.WriteLine("Seed user creation failed: " + string.Join(", ", createResult.Errors.Select(e => e.Description)));
            }
        }
        if (isAdmin && !await userManager.IsInRoleAsync(user, adminRole))
        {
            await userManager.AddToRoleAsync(user, adminRole);
        }
    }

    await EnsureUser("oli", "oli@example.com", isAdmin: true);
    await EnsureUser("alice", "alice@example.com", isAdmin: false);
    await EnsureUser("bob", "bob@example.com", isAdmin: false);
}

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