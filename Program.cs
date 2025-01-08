using System.Text;
using dotenv.net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

var ENV = DotEnv.Read();

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
        ValidIssuer = ENV["ISSUER"], // Replace with your issuer
        ValidAudience = ENV["AUDIENCE"], // Replace with your audience
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ENV["SECRET_KEY"])) // Replace with your own sign key
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
