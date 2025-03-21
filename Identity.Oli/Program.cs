using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.Oli.Data;
using Identity.Oli.QuickStart;
using Identity.Oli.Services;
using Identity.Oli.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<GoalRequestValidator>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddDeveloperSigningCredential();

// Dependency Injection for application services
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IGoalsRepository, GoalsRepository>();
builder.Services.AddScoped<IGoalService, GoalService>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ReadPolicy", policy =>
        policy.RequireClaim("scope", "api1.read"));
    options.AddPolicy("WritePolicy", policy =>
        policy.RequireClaim("scope", "api1.write"));
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireClaim("scope", "api1.admin"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();
app.MapControllers();


app.Run();
