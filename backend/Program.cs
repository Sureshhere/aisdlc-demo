var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
   .WithName("HealthCheck");

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// Vulnerable endpoint - intentional security issues for Claude to catch
app.MapPost("/login", (string username, string password) =>
{
    // ISSUE 1: Hardcoded credentials (security risk)
    if (username == "admin" && password == "password123")
    {
        // ISSUE 2: No error handling
        var token = "secret_token_" + Guid.NewGuid().ToString();
        return Results.Ok(new { token = token, message = "Login successful" });
    }
    
    // ISSUE 3: Missing input validation - no null checks
    return Results.Unauthorized();
})
.WithName("Login")
.WithOpenApi();

// Password reset endpoint with vulnerabilities
app.MapPost("/reset-password", (string email) =>
{
    // VULNERABILITY 1: No rate limiting - allows brute force attacks
    // VULNERABILITY 2: No email verification - information disclosure (confirms if email exists)
    // VULNERABILITY 3: No CSRF token validation
    // VULNERABILITY 4: Accepts any email without validation
    
    if (string.IsNullOrEmpty(email))
        return Results.BadRequest("Email is required");
    
    // Always returns success, revealing if email exists in system
    return Results.Ok(new { message = "If this email is registered, you'll receive a password reset link", email = email });
})
.WithName("ResetPassword")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
