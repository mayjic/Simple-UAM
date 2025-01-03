using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "localtest", // Replace with your issuer
        ValidAudience = "testlocal", // Replace with your audience
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fd55762c9e0864c7915fad1bac627d57")) // Replace with your own sign key
    };
});

// Add authorization
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication(); // Authenticate the user
app.UseAuthorization();  // Check permissions
app.MapControllers();    // Map API endpoints

app.Run();
