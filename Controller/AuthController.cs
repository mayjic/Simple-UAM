using dotenv.net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        
        var ENV = DotEnv.Read();
        // Replace this with your actual user validation logic
        if (request.user_name == "admin" && request.pass == "password")
        {
            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("fd55762c9e0864c7915fad1bac627d57"); 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, request.user_name),
                    new Claim(ClaimTypes.Role, "Admin") 
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = ENV["ISSUER"],
                Audience = ENV["AUDIENCE"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token) });
        }

        return Unauthorized("Invalid credentials");
    }

    [HttpPost("check-token")]
    public IActionResult CheckToken([FromBody] BodyToken bodyToken)
    {
        if (bodyToken == null || string.IsNullOrEmpty(bodyToken.token))
        {
            return BadRequest(new { Error = "The token field is required." });
        }

        if (ValidateToken(bodyToken.token))
        {
            return Ok(new { Message = "Token is valid" });
        }

        return Unauthorized("Token is invalid");
    }

    private bool ValidateToken(string token)
    {
        
        var ENV = DotEnv.Read();
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("fd55762c9e0864c7915fad1bac627d57");

        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = ENV["ISSUER"],
                ValidAudience = ENV["AUDIENCE"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            return true;
        }
        catch (SecurityTokenException)
        {
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }


}

public class LoginRequest
{
    public string user_name { get; set; }
    public string pass { get; set; }
}

public class BodyToken
{
    public string token { get; set; }
}


