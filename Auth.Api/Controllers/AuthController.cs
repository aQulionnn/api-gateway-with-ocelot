using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost]
    [Route("sign-in")]
    public IActionResult Signin()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("your-32-character-long-secret-key!!");
        
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity([ new Claim("sub", Guid.NewGuid().ToString()) ]),
            Expires = DateTime.UtcNow.AddHours(1),
            Audience = "api-gateway",
            Issuer = "https://localhost:5003",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { AccessToken = tokenHandler.WriteToken(token) });
        
        
        // return SignIn (
        //     new ClaimsPrincipal (
        //         new ClaimsIdentity (
        //             [
        //                 new Claim("sub", Guid.NewGuid().ToString())
        //             ],
        //             JwtBearerDefaults.AuthenticationScheme
        //         )
        //     ),
        //     authenticationScheme: JwtBearerDefaults.AuthenticationScheme
        // );
    }
}