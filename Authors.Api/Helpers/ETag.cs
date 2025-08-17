using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Authors.Api.Helpers;

public static class ETag
{
    public static string Generate(object data)
    {
        var json = JsonSerializer.Serialize(data);
        
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(json));
        var hash = Convert.ToBase64String(hashBytes);
        
        return  $"\"{hash}\"";
    }
}