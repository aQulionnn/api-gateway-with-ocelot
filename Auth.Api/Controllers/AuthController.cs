using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Api.Contracts;
using Auth.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IAesService aesService) : ControllerBase
{
    private readonly IAesService _aesService = aesService;
    
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
    }

    [HttpPost]
    [Route("encrypt")]
    public IActionResult Encrypt([FromBody] EncryptRequest request)
    {
        try
        {
            var (key, iv) = _aesService.GenerateKeyIv();
            var encrypted = _aesService.Encrypt(request.Data, key, iv);

            return Ok(new
            {
                EncryptedData = Convert.ToBase64String(encrypted),
                Key = Convert.ToBase64String(key),
                Iv = Convert.ToBase64String(iv)
            });
        }
        catch (CryptographicException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route("decrypt")]
    public IActionResult Decrypt([FromBody] DecryptRequest request)
    {
        try
        {
            var key = Convert.FromBase64String(request.Key);
            var iv = Convert.FromBase64String(request.IV);
            var data = Convert.FromBase64String(request.EncryptedData);

            var decrypted = _aesService.Decrypt(data, key, iv);
            return Ok(new
            {
                DecryptedData = decrypted,
            });
        }
        catch (CryptographicException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}