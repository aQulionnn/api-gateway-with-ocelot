using System.Security.Cryptography;

namespace Auth.Api.Services;

internal sealed class AesService : IAesService
{
    public (byte[] Key, byte[] Iv) GenerateKeyIv()
    {
        using var aes = Aes.Create();
        return (aes.Key, aes.IV);
    }

    public byte[] Encrypt(string plainText, byte[] key, byte[] iv)
    {
        try
        {
            using var aes = Aes.Create();

            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            using var memoryStream = new MemoryStream();
            using (var encryptor = aes.CreateEncryptor())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                streamWriter.Write(plainText);
            }

            return memoryStream.ToArray();
        }
        catch (CryptographicException ex)
        {
            throw new CryptographicException("Failed to encrypt data", ex.Message);
        }
    }

    public string Decrypt(byte[] cipherText, byte[] key, byte[] iv)
    {
        try
        {
            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = key;
            aes.IV = iv;

            using var memoryStream = new MemoryStream(cipherText);
            using var decryptor = aes.CreateDecryptor();
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }
        catch (CryptographicException ex)
        {
            throw new CryptographicException("Failed to decrypt data", ex.Message);
        }
    }
}

public interface IAesService
{
    public (byte[] Key, byte[] Iv) GenerateKeyIv();
    public byte[] Encrypt(string plainText, byte[] key, byte[] iv);
    public string Decrypt(byte[] cipherText, byte[] key, byte[] iv);
}