using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Api.Contracts;
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
    }

    [HttpPost]
    [Route("encrypt")]
    public IActionResult Encrypt([FromBody] EncryptRequest request)
    {
        var plainText = request.Data;
        var masterKey = Convert.FromBase64String(request.MasterKey);
        var result = string.Empty;

        try
        {
            using var aes = Aes.Create();

            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = masterKey;
            aes.IV = RandomNumberGenerator.GetBytes(16);

            using var memoryStream = new MemoryStream();
            memoryStream.Write(aes.IV, 0, aes.IV.Length);

            using (var encryptor = aes.CreateEncryptor())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                streamWriter.Write(plainText);
            }

            result = Convert.ToBase64String(memoryStream.ToArray());
        }
        catch (CryptographicException ex)
        {
            return BadRequest(ex.Message);
        }
        
        return Ok(result);
    }

    [HttpPost]
    [Route("decrypt")]
    public IActionResult Decrypt([FromBody] DecryptRequest request)
    {
        var cipherData = Convert.FromBase64String(request.EncryptedData);
        var masterKey = Convert.FromBase64String(request.MasterKey);
        var result = string.Empty;

        try
        {
            if (cipherData.Length < 16)
            {
                return BadRequest("Invalid encrypted data format");
            }

            byte[] iv = new byte[16];
            byte[] encryptedData = new byte[cipherData.Length - 16];

            Buffer.BlockCopy(cipherData, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(cipherData, 16, encryptedData, 0, encryptedData.Length);

            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = masterKey;
            aes.IV = iv;

            using var memoryStream = new MemoryStream(encryptedData);
            using var decryptor = aes.CreateDecryptor();
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            result = streamReader.ReadToEnd();
        }
        catch (CryptographicException ex)
        {
            return BadRequest(ex.Message);
        }
        
        return Ok(result);
    }
}