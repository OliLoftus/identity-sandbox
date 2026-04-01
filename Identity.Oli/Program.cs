using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.Oli.Data;
using Identity.Oli.Models.Responses;
using Identity.Oli.Services;
using Identity.Oli.Validation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

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
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(entry => entry.Value?.Errors.Count > 0)
                .ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value!.Errors.Select(error => error.ErrorMessage).ToArray());

            return new BadRequestObjectResult(new ErrorResponse(
                "ValidationError",
                "Validation failed.",
                errors));
        };
    });

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
    options.AddPolicy("DeletePolicy", policy =>
        policy.RequireClaim("scope", "api1.delete"));
});

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        if (exceptionHandler?.Error != null)
        {
            logger.LogError(exceptionHandler.Error, "Unhandled exception");
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var message = app.Environment.IsDevelopment()
            ? exceptionHandler?.Error.Message ?? "An unexpected error occurred."
            : "An unexpected error occurred.";

        await context.Response.WriteAsJsonAsync(new ErrorResponse("Error", message));
    });
});

app.UseRouting();

// Apply the CORS policy
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
