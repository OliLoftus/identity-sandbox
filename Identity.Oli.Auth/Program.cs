using Duende.IdentityServer;
using Identity.Oli.Auth;
using Identity.Oli.Auth.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryApiResources(Config.ApiResources)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddDeveloperSigningCredential();

var app = builder.Build();
var (verifier, challenge) = PkceHelper.GeneratePkceValues();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Code Verifier: {verifier}", verifier);
logger.LogInformation("Code Challenge: {challenge}", challenge);

app.UseIdentityServer();
app.Run();