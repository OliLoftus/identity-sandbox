using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.Oli.Data;
using Identity.Oli.QuickStart;
using Identity.Oli.Services;
using Identity.Oli.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var spaClientUrl = "http://localhost:5173";

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(spaClientUrl)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 1. Add authentication services
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        // The URL of your IdentityServer
        options.Authority = "https://localhost:7040";

        // The name of the API resource you defined in Config.cs
        options.Audience = "api1";
    });

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<GoalRequestValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// Apply the CORS policy
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
